using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientAppTest.Converters
{
    [TestClass]
    public class EnumToVisibilityConverterTest
    {
        internal enum ExampleEnumeration { Hearts, Spades, Clubs, Diamonds }

        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new EnumToVisibilityConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_Match_Should_Return_Visible()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(ExampleEnumeration.Spades, null, "Spades", null);

            // assert
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void Convert_NotMatch_Should_Return_Collapsed()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(ExampleEnumeration.Diamonds, null, "Spades", null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_Null_Should_Return_Collapsed()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(null, null, "Spades", null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_ParameterNull_Should_Return_Collapsed()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_ParameterNotString_Should_Return_Collapsed()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(123, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_ParameterNotEnumValue_Should_Return_Collapsed()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act
            var result = target.Convert(ExampleEnumeration.Hearts, null, "Grapes", null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }


        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new EnumToVisibilityConverter();

            // act - should throw
            var result = target.ConvertBack(null, null, null, null);
        }
    }
}
