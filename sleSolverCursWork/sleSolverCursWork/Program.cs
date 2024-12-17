using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace sleSolverCursWork
{
    internal class Program
    {
        const int maxSizeOfArray = 199999999;
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args)) // Инициализация MPI
            {
                Intracommunicator comm = Communicator.world;
                int rank = comm.Rank; // Ранг текущего процесса
                int size = comm.Size; // Общее количество процессов

                int n = 1;
                double[] a = new double[1];
                double[] b = new double[1]; 

                if (rank == 0)
                {
                    Console.WriteLine("Выберите способ ввода матрицы:");
                    Console.WriteLine("1. Чтение из файла.");
                    Console.WriteLine("2. Ввод с консоли.");

                    int t = int.Parse(Console.ReadLine());
                    if (t == 1)
                    {
                        MatrixConsoleInput.GetDataFromFile();
                    }
                    else
                    {
                        Console.WriteLine("Введите размерность матрицы: ");
                        n = int.Parse(Console.ReadLine());
                        a = MatrixConsoleInput.Input_A_Coefficients(n);
                        b = MatrixConsoleInput.Input_B_Coefficients(n);

                        Console.WriteLine("Нажмите любую клавишу для начала вычисления.");
                        string pause = Console.ReadLine();
                    }
                }
                comm.Broadcast(ref n, 0);

                double[] outer = new double[1];

                if (rank == 0)
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    Console.WriteLine("n = " + n);
                    outer = Dot_mpi_root(a, b, n, n, 1, size);

                    timer.Stop();
                    string timeOfSolving = GetElapsedTime(timer);
                    Console.WriteLine(timeOfSolving);

                    for (int i = 0; i < outer.Length; i++)
                    {
                        Console.Write(outer[i] + " ");
                    }
                }
                else
                {
                    Console.WriteLine("n = " + n);
                    Dot_Mpi(n, n, 1, rank, size);
                }
            }
        }

        #region Math
        private static double[] Dot_mpi_root(double[] a, double[] b, int n, int m, int p, int size)
        {
            double[] outResult = new double[n * p];
            for (int i = 0; i < n * p; i++) outResult[i] = 0.0;

            double[] aT = Transpose(a, n, m);
            int h = m / size;

            for (int i = h; i < m; i += h)
            {
                double[] merged = new double[h * (n + p)];
                Array.Copy(aT, i * n, merged, 0, h * n);
                Array.Copy(b, i * p, merged, h * n, h * p);
                Communicator.world.Send<double[]>(merged, i / h, i / h);
            }

            for (int i1 = 0; i1 < h; i1++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < p; k++)
                    {
                        outResult[j * p + k] += aT[i1 * n + j] * b[i1 * p + k];
                    }
                }
            }

            for (int stage = 1; stage < size; stage *= 2)
            {
                double[] outReceived = new double[n * p];
                Communicator.world.Receive<double[]>(stage, stage, out outReceived);
                for (int j = 0; j < n * p; j++)
                {
                    outResult[j] += outReceived[j];
                }
            }

            return outResult;
        }

        private static double[] Transpose(double[] a, int n, int m)
        {
            double[] b = new double[n * m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    b[j * n + i] = a[i * m + j];
                }
            }
            return b;
        }

        private static void Dot_Mpi(int n, int m, int p, int rank, int size)
        {
            int h = m / size;

            double[] a_r = new double[h * n];
            double[] b_r = new double[h * p];
            double[] merged = new double[h * (n + p)];

            Communicator.world.Receive<double[]>(0, rank, out merged);

            Array.Copy(merged, a_r, h * n);
            Array.Copy(merged, h * n, b_r, 0, h * p);

            double[] outResult = new double[n * p];
            for (int i = 0; i < n * p; i++)
            {
                outResult[i] = 0.0;
            }

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < p; k++)
                    {
                        outResult[j * p + k] += a_r[i * n + j] * b_r[i * p + k];
                    }
                }
            }

            for (int stage = 1; stage < size; stage *= 2)
            {
                if (rank % stage == 0 && rank - stage >= 0 && (rank - stage) % (stage * 2) == 0)
                {
                    Communicator.world.Send<double[]>(outResult, rank - stage, rank);
                }
                else if (rank % stage == 0 && rank + stage < size && rank % (stage * 2) == 0)
                {
                    double[] outReceived = new double[n * p];
                    Communicator.world.Receive<double[]>(rank + stage, rank + stage, out outReceived);

                    for (int j = 0; j < n * p; j++)
                    {
                        outResult[j] += outReceived[j];
                    }
                }
            }
        }
        #endregion

        private static string GetElapsedTime(Stopwatch stopwatch)
        {
            TimeSpan timeElapsed = stopwatch.Elapsed;
            double milliseconds = timeElapsed.TotalMilliseconds;
            int minutes = timeElapsed.Minutes;
            int seconds = timeElapsed.Seconds;

            string result = "";
            result += "\n" + minutes + " минут; " + seconds + " секунд; " + milliseconds.ToString("0.###") + " милисекунд.";

            return result;
        }
    }
}
