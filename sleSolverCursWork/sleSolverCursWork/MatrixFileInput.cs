using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    public class MatrixFileInput
    {
        public static double[,] GetMatrixFromFile(string path)
        {
            try
            {
                
                Console.WriteLine(GetNumberOfLines(path).ToString());

                string[] lines = File.ReadAllLines(path);
                int matSize = lines.Length;
                double[,] matrix = new double[matSize, matSize];
                for (int i = 0; i < matSize; i++)
                {
                    string[] coeffs = NormalizeSpaces(lines[i]).Split(' ');
                    for (int j = 0; j < coeffs.Length; j++)
                    {
                        matrix[i, j] = double.Parse(coeffs[j]);
                    }
                }

                return matrix;
            }
            catch (Exception e)
            {
                throw new Exception("Файл с матрицей по заданому пути не найден или входная строка имела не верный формат.");
            }
        }

        public static double[] GetVectorFromFile(string path)
        {
            try
            {
                //path = @"C:\Учёба\7 семестр\РИС\sleSolverCursWork\FileB.txt";
                string[] lines = File.ReadAllLines(path);
                double[] vector = new double[lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    vector[i] = double.Parse(NormalizeSpaces(lines[i]));
                }

                return vector;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static string NormalizeSpaces(string text)
        {
            return System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
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
