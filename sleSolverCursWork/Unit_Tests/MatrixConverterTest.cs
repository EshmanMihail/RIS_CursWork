using Microsoft.VisualStudio.TestTools.UnitTesting;
using sleSolverCursWork;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class MatrixConverterTest
    {
        [TestMethod]
        public void StringToMatrix_SimpleCase_ReturnsCorrectResult()
        {  
            string serializedData = "1 2\n3 4";
            double[,] expected = { { 1.0, 2.0 }, { 3.0, 4.0 } };

            double[,] result = MatrixConverter.StringToMatrix(serializedData);

            CollectionAssert.AreEqual(expected, result);
        }
        
        [TestMethod] 
        public void StringToMatrix_WithSpaces_ReturnsCorrectResult() { 
            string serializedData = "1 2\n3 4";
            double[,] expected = { { 1.0, 2.0 }, { 3.0, 4.0 } };

            double[,] result = MatrixConverter.StringToMatrix(serializedData);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringToMatrix_LargeMatrix_ReturnsCorrectResult() {
            string serializedData = "1 2 3\n4 5 6\n7 8 9";
            double[,] expected = { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 }, { 7.0, 8.0, 9.0 } }; 

            double[,] result = MatrixConverter.StringToMatrix(serializedData); 

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
