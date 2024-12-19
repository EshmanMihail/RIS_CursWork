using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using MathNet.Numerics.LinearAlgebra;
using System.Threading;

namespace sleSolverCursWork
{
    internal class Program
    {
        const int maxSizeOfArray = 199999999;
        public static int procCount = 0;

        private static bool isRunning = true;
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args)) // Инициализация MPI
            {
                Intracommunicator comm = Communicator.world;
                int rank = comm.Rank; // Ранг текущего процесса
                int size = comm.Size; // Общее количество процессов
                procCount = size;

                int t = 2;

                int n = 1;
                double[,] matrix = new double[1, 1];
                double[] b = new double[1]; 

                if (rank == 0)
                {
                    ConsoleMenu();
                    t = int.Parse(Console.ReadLine());
                    if (t == 1)
                    {
                        FileInput(out matrix, out n, out b);
                    }
                    else if (t == 2)
                    {
                        ConsoleInput(out matrix, out n, out b);
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод.");
                    }
                }
                comm.Broadcast(ref n, 0);

                double[] outer = new double[1];
                if (rank == 0)
                {
                    Stopwatch timer = new Stopwatch();
                    timer.Start();
                    Console.WriteLine("Вычисление...");

                    double[] aInversed = СalculateInverseMatrix(matrix);
                    outer = Dot_mpi_root(aInversed, b, n, n, 1, size);

                    timer.Stop();
                    string timeOfSolving = GetElapsedTime(timer);

                    Console.WriteLine(timeOfSolving);
                    if (t == 1)
                    {
                        string pathToResultFile = @"C:\Учёба\7 семестр\РИС\sleSolverCursWork\FileResult.txt";
                        ResultWriter.WriteResultInFile(pathToResultFile, outer);
                        Console.WriteLine("Результат был записан в файл.");
                    }
                    else PrintResultVectorOnConsole(outer);
                }
                else
                {
                    Dot_Mpi(n, n, 1, rank, size);
                }
            }
        }

        private static void ConsoleMenu()
        {
            Console.WriteLine("Выберите способ ввода матрицы:");
            Console.WriteLine("1. Чтение из файла.");
            Console.WriteLine("2. Ввод с консоли.");
        }

        private static void FileInput(out double[,] matrix, out int n, out double[] b)
        {
            Console.WriteLine("Введите путь к файлу с кофициентами A:");
            string pathA = Console.ReadLine();

            if (pathA == "back" || pathA == "b")
            {
                Console.Clear();
                matrix = new double[1, 1];
                n = 1;
                b = new double[1];
                return;
            }

            Console.WriteLine("Введите путь к файлу с кофициентами B:");
            string pathB = Console.ReadLine();

            matrix = MatrixFileInput.GetMatrixFromFile(pathA);
            n = matrix.GetLength(0);
            b = MatrixFileInput.GetVectorFromFile(pathB);

            Console.WriteLine("Нажмите любую клавишу для начала вычисления.");
            Console.ReadLine();
        }

        private static void ConsoleInput(out double[,] matrix, out int n, out double[] b)
        {
            Console.WriteLine("Введите размерность матрицы: ");
            n = int.Parse(Console.ReadLine());
            matrix = MatrixConsoleInput.Input_A_Coefficients(n);
            b = MatrixConsoleInput.Input_B_Coefficients(n);

            Console.WriteLine("Нажмите любую клавишу для начала вычисления.");
            Console.ReadLine();
        }

        private static double[] СalculateInverseMatrix(double[,] matrix)
        {
            var numMatrix = Matrix<double>.Build.DenseOfArray(matrix);
            Matrix<double> inversed = numMatrix.Inverse();
            return MatrixToFlatArray(inversed);
        }

        private static double[] MatrixToFlatArray(Matrix<double> matrix)
        {
            int rows = matrix.RowCount;
            int cols = matrix.ColumnCount;

            double[] flatArray = new double[rows * cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flatArray[i * cols + j] = matrix[i, j];
                }
            }

            return flatArray;
        }

        private static void PrintResultVectorOnConsole(double[] outer)
        {
            for (int i = 0; i < outer.Length; i++)
            {
                if (Math.Abs(outer[i]) < 1e-10)
                {
                    Console.Write(0 + " ");
                }
                else
                {
                    Console.Write(outer[i].ToString("0.000") + " ");
                }
            }
        }

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
    }
}
