using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    public class MatrixConverter
    {
        public static double[] StringToMatrixOfVector(string serializedData)
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

        public static double[,] StringToMatrix(string serializedData)
        {
            serializedData = serializedData.Trim();
            string[] lines = serializedData.Trim().Split('\n');
            int rows = lines.Length;
            int cols = lines[0].Split(' ').Length;
            double[,] deserializedMatrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string[] elements = lines[i].Split(' ');
                for (int j = 0; j < cols; j++)
                {
                    deserializedMatrix[i, j] = double.Parse(elements[j]);
                }
            }

            return deserializedMatrix;
        }

        public static double[] StringToVector(string vector)
        {
            vector = vector.Trim();
            string[] vectorElements = vector.Split(' ');
            double[] deserializedVector = new double[vectorElements.Length];
            for (int i = 0; i < vectorElements.Length; i++)
            {
                deserializedVector[i] = double.Parse(vectorElements[i]);
            }
            return deserializedVector;
        }
    }
}
