using Xunit;
using System;
using System.Windows;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    
    public class BooleanToVisibilityConverterTest
    {
        [Fact]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new BooleanToVisibilityConverter();

            Assert.IsAssignableFrom<IValueConverter>(target);
        }

        [Fact]
        public void Convert_True_Should_Return_Visible()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();
            bool value = true;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.Equal(Visibility.Visible, result);
        }

        [Fact]
        public void Convert_False_Should_Return_Collapsed()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();
            bool value = false;

            // act
            var result = target.Convert(value, null, null, null);

            // assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        [Fact]
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
            Assert.Equal(Visibility.Hidden, result);
        }

        [Fact]
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
            Assert.Equal(Visibility.Visible, result);
        }

        [Fact]
        public void Convert_ParameterNotBooleanValue_Should_Return_Collapsed()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();

            // act
            var result = target.Convert(123, null, null, null);

            // assert
            Assert.Equal(Visibility.Collapsed, result);
        }

        [Fact]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new BooleanToVisibilityConverter();

            // act - should throw
            Assert.Throws<NotImplementedException>(() => target.ConvertBack(null, null, null, null));
        }
    }
}
