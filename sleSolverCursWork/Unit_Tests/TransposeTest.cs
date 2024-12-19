using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class TransposeTest
    {
        [TestMethod]
        public void Transpose_SquareMatrix_ReturnsCorrectResult()
        {
            double[] matrix = { 1, 2, 3, 4 };
            double[] expected = { 1, 3, 2, 4 };
            int n = 2, m = 2;

            double[] result = Transpose(matrix, n, m);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transpose_RectangularMatrix_ReturnsCorrectResult()
        {
          double[] matrix = { 1, 2, 3, 4, 5, 6 };
          double[] expected = { 1, 4, 2, 5, 3, 6 };
          int n = 2, m = 3;

          double[] result = Transpose(matrix, n, m);
          CollectionAssert.AreEqual(expected, result);
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
    }
}
