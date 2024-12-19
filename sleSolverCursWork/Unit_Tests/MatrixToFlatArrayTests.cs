using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathNet.Numerics.LinearAlgebra;

namespace Unit_Tests
{
    [TestClass]
    public class MatrixToFlatArrayTests
    {
        [TestMethod]
        public void MatrixToFlatArray_SimpleCase_ReturnsCorrectResult()
        {
            var matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                { 1.0, 2.0 },
                { 3.0, 4.0 }
            });
            double[] expected = { 1.0, 2.0, 3.0, 4.0 };

            double[] result = MatrixToFlatArray(matrix);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MatrixToFlatArray_LargerMatrix_ReturnsCorrectResult()
        {
            var matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                { 1.0, 2.0, 3.0 },
                { 4.0, 5.0, 6.0 },
                { 7.0, 8.0, 9.0 }
            });
            double[] expected = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 };

            double[] result = MatrixToFlatArray(matrix);

            CollectionAssert.AreEqual(expected, result);
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
    }
}
