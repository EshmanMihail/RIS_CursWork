using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    internal class MatrixFileInput
    {
        public static double[,] GetMatrixFromFile(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                int matSize = lines.Length;
                double[,] matrix = new double[matSize, matSize];
                for (int i = 0; i < matSize; i++)
                {
                    string[] coeffs = lines[i].Split(' ');
                    for (int j = 0; j < coeffs.Length; j++)
                    {
                        matrix[i, j] = double.Parse(coeffs[j]);
                    }
                }

                return matrix;
            }
            catch (Exception e)
            {
                throw new Exception("Файл по заданому пути не найден или входная строка имела не верный формат.");
            }
        }

        public static double[] GetVectorFromFile(string path)
        {
            try
            {
                //path = @"C:\Учёба\7 семестр\РИС\sleSolverCursWork\FileB.txt";
                string text = File.ReadAllText(path);
                string[] values = text.Split('\n');
                double[] vector = new double[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    vector[i] = double.Parse(values[i]);
                }

                return vector;
            }
            catch (Exception e)
            {
                throw new Exception("Файл по заданому пути не найден или входная строка имела не верный формат.");
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
    }
}
