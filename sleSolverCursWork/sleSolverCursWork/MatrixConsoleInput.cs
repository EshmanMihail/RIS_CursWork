﻿using System;
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
        public static void GetDataFromFile()
        {
            try
            {
                Console.WriteLine("Введите путь к файлу:");
                string path = Console.ReadLine();

                int lineCount = GetNumberOfLines(path);

                string[] strs = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                throw new Exception("Файл по заданому пути не найден.");
            }
        }

        private static int GetNumberOfLines(string path)
        {
            int lineCount = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }

        public static double[] Input_A_Coefficients(int n)
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