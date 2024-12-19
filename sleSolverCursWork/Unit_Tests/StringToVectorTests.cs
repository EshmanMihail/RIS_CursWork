using Microsoft.VisualStudio.TestTools.UnitTesting;
using sleSolverCursWork;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class StringToVectorTests
    {
        [TestMethod]
        public void StringToVector_SimpleCase_ReturnsCorrectResult()
        { 
            string vector = "1 2 3";
            double[] expected = { 1.0, 2.0, 3.0 };

            double[] result = MatrixConverter.StringToVector(vector); 

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod] 
        public void StringToVector_WithSpaces_ReturnsCorrectResult() 
        { 
            string vector = " 1 2 3 ";
            double[] expected = { 1.0, 2.0, 3.0 }; 

            double[] result = MatrixConverter.StringToVector(vector); 

            CollectionAssert.AreEqual(expected, result); 
        }
        [TestMethod] 
        public void StringToVector_NegativeNumbers_ReturnsCorrectResult() 
        { 
            string vector = "-1 -2 -3  ";
            double[] expected = { -1.0, -2.0, -3.0 };

            double[] result = MatrixConverter.StringToVector(vector);

            CollectionAssert.AreEqual(expected, result); 
        }
    }
}
