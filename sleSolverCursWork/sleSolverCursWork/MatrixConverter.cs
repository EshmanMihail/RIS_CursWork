using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    internal class MatrixConverter
    {
        public static double[] StringToMatrix(string serializedData)
        {
            string[] lines = serializedData.Split('\n');

            int n = lines.Length;
            double[] deserializedMatrix = new double[n * n];

            for (int i = 0; i < n; i++)
            {
                string[] elements = lines[i].Split(' ');
                for (int j = 0; j < n; j++)
                {
                    deserializedMatrix[i * n + j] = double.Parse(elements[j]);
                }
            }
            return deserializedMatrix; 
        }

        public static double[] StringToVector(string vector)
        {
            string[] vectorElements = vector.Split(' ');
            double[] deserializedVector = new double[vectorElements.Length];
            for (int i = 0; i < vectorElements.Length; i++)
            {
                deserializedVector[i] = double.Parse(vectorElements[i]);
            }
            return deserializedVector;
        }

        private static string VectorToString(double[,] A, double[] B)
        {
            StringBuilder sb = new StringBuilder();

            int n = A.GetLength(0);
            sb.AppendLine(n.ToString());
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sb.Append(A[i, j]);
                    if (j < n - 1)
                        sb.Append(" ");
                }
                sb.AppendLine();
            }

            for (int i = 0; i < B.Length; i++)
            {
                sb.Append(B[i]);
                if (i < B.Length - 1)
                    sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
