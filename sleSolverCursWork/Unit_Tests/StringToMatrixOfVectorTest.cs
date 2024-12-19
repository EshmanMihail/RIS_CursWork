using Microsoft.VisualStudio.TestTools.UnitTesting;
using sleSolverCursWork;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class StringToMatrixOfVectorTests
    {
        [TestMethod]
        public void StringToMatrixOfVector_SimpleCase_ReturnsCorrectResult()
        {
            string serializedData = "1 2\n3 4";
            double[] expected = { 1.0, 2.0, 3.0, 4.0 };

            double[] result = MatrixConverter.StringToMatrixOfVector(serializedData);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringToMatrixOfVector_LargerMatrix_ReturnsCorrectResult()
        {
            string serializedData = "1 2 3\n4 5 6\n7 8 9";
            double[] expected = { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 };

            double[] result = MatrixConverter.StringToMatrixOfVector(serializedData);

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
