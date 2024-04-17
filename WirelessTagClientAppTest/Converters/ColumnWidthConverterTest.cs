using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    [TestClass]
    public class ColumnWidthConverterTest
    {
        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new ColumnWidthConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_True_Should_Return_ParameterisedWidth()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(true, null, 100d, null);

            // assert
            Assert.AreEqual(100d, result);
        }

        [TestMethod]
        public void Convert_False_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(false, null, 100d, null);

            // assert
            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        public void Convert_ValueNotBoolean_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert("hello", null, 100d, null);

            // assert
            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        public void Convert_ParameterNotDouble_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(false, null, "hello", null);

            // assert
            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act - should throw
            var result = target.ConvertBack(null, null, null, null);
        }
    }
}
