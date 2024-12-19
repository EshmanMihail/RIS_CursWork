using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    public class MatrixConsoleInput
    {
        public static double[,] Input_A_Coefficients(int n)
        {
            string A = "";
            Console.WriteLine("Введите матрицу коэффициентов:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Введите числа строки {i + 1}:");
                string line = "";
                if (i == n - 1) line = Console.ReadLine().Trim();
                else line = Console.ReadLine().Trim() + "\n";
                A += line;
            }
            //double[,] matrix = MatrixConverter.StringToMatrix(A);
            //return SolvingHelper.AddZeroColumns(matrix, Program.procCount);
            return MatrixConverter.StringToMatrix(A);
        }

        public static double[] Input_B_Coefficients(int n)
        {
            string B = "";
            Console.WriteLine("Введите вектор свободных членов:");
            string lineB = Console.ReadLine().Trim();
            B += lineB;
            return MatrixConverter.StringToVector(B);
        }
    }
}
