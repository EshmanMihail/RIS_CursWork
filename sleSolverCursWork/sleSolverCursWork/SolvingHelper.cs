using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    internal class SolvingHelper
    {
        public static double[,] AddZeroColumns(double[,] matrix, int processCount)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int remainderCols = cols % processCount;
            int additionalCols = (remainderCols == 0) ? 0 : processCount - remainderCols;
            int newCols = cols + additionalCols;

            int additionalRows = newCols - rows;
            int newRows = rows + (additionalRows > 0 ? additionalRows : 0);

            double[,] newMatrix = new double[newRows, newCols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }

            return newMatrix;
        }
    }
}
