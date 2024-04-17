using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    [TestClass]
    public class BooleanToVisibilityConverterTest
    {
        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new BooleanToVisibilityConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_True_Should_Return_Visible()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();
            bool value = true;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void Convert_False_Should_Return_Collapsed()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();
            bool value = false;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void Convert_Inverted_True_Should_Return_Visible()
        {
            // arrange
            var target = new BooleanToVisibilityConverter()
            { 
                VisibilityTrue = Visibility.Hidden,
                VisibilityFalse = Visibility.Visible
            };

            bool value = true;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Hidden, result);
        }

        [TestMethod]
        public void Convert_Inverted_False_Should_Return_Collapsed()
        {
            // arrange
            var target = new BooleanToVisibilityConverter()
            {
                VisibilityTrue = Visibility.Hidden,
                VisibilityFalse = Visibility.Visible
            };

            bool value = false;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void Convert_ParameterNotBooleanValue_Should_Return_Collapsed()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();

            // act
            var result = target.Convert(123, null, null, null);

            // assert
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();

            // act - should throw
            var result = target.ConvertBack(null, null, null, null);
        }
    }
}
