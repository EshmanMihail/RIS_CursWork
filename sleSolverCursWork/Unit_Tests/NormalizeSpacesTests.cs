using Microsoft.VisualStudio.TestTools.UnitTesting;
using sleSolverCursWork;
using System;

namespace Unit_Tests
{
    [TestClass]
    public class NormalizeSpacesTests
    {
        [TestMethod]
        public void NormalizeSpaces_MultipleSpaces_ReturnsNormalizedString()
        { 
            string input = "This    is   a    test.";
            string expected = "This is a test."; 

            string result = MatrixFileInput.NormalizeSpaces(input);

            Assert.AreEqual(expected, result); 
        } 

        [TestMethod] 
        public void NormalizeSpaces_TrailingLeadingSpaces_ReturnsTrimmedString() 
        { 
            string input = "   This is a test.    ";
            string expected = "This is a test.";

            string result = MatrixFileInput.NormalizeSpaces(input);

            Assert.AreEqual(expected, result); 
        }

        [TestMethod] 
        public void NormalizeSpaces_NewlineTabs_ReturnsNormalizedString() 
        { 
            string input = "This is\na\ttest.";
            string expected = "This is a test."; 

            string result = MatrixFileInput.NormalizeSpaces(input); 

            Assert.AreEqual(expected, result); 
        } 

        [TestMethod] 
        public void NormalizeSpaces_SingleWord_ReturnsSameWord() 
        { 
            string input = "Word";
            string expected = "Word";

            string result = MatrixFileInput.NormalizeSpaces(input);

            Assert.AreEqual(expected, result); }
        }
}
