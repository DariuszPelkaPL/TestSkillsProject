using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalCurrencyToTextConverter.BusinessLogic;

namespace UnitTestProject
{
    using System;

    [TestClass]
    public class CurrencyConverterUnitTest
    {
        [TestMethod]
        [DataTestMethod]
        [DataRow("0", "zero dollars")]
        [DataRow("1", "one dollar")]
        [DataRow("25,1", "twenty-five dollars and ten cents")]
        [DataRow("0,01", "zero dollars and one cent")]
        [DataRow("999 999 999,99", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [DataRow("123", "one hundred twenty-three dollars")]
        [DataRow("235", "two hundred thirty-five dollars")]
        [DataRow("1 234", "one thousand two hundred thirty-four dollars")]
        [DataRow("3 715", "three thousand seven hundred fifteen dollars")]
        [DataRow("1 266 411", "one million two hundred sixty-six thousand four hundred eleven dollars")]
        [DataRow("19 516 411", "nineteen million five hundred sixteen thousand four hundred eleven dollars")]
        public void CheckConversion_Correct_WhenValidDataProvided(string number, string amountInWords)
        {
            // Arrange
            var converter = new CurrencyConverter();

            // Act
            var result = converter.ConvertToWords(number);

            // Assert
            Assert.AreEqual(amountInWords, result);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("0,88.9")]
        [DataRow("a")]
        public void CheckConversion_Error_WhenProvidingIncoorectlyFormattedValue(string number)
        {
            // Arrange
            var converter = new CurrencyConverter();
            bool exceptionThrown = false;

            // Act
            try
            {
                var result = converter.ConvertToWords(number);
            }
            catch (ArgumentException exc)
            {
                exceptionThrown = true;
                Assert.AreEqual("Provided amount has incorrect format", exc.Message);
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void CheckConversion_Error_WhenProvidingTooBigValue()
        {
            // Arrange
            var converter = new CurrencyConverter();
            bool exceptionThrown = false;

            // Act
            try
            {
                var result = converter.ConvertToWords("1000000000");
            }
            catch (ArgumentException exc)
            {
                exceptionThrown = true;
                Assert.AreEqual("Too large amount", exc.Message);
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void CheckConversion_Error_WhenProvidingTooLongFractionalPart()
        {
            // Arrange
            var converter = new CurrencyConverter();
            bool exceptionThrown = false;

            // Act
            try
            {
                var result = converter.ConvertToWords("12,123");
            }
            catch (ArgumentException exc)
            {
                exceptionThrown = true;
                Assert.AreEqual("Too large fractional part", exc.Message);
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }
    }
}
