using Xunit;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    
    public class ColumnWidthConverterTest
    {
        [Fact]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new ColumnWidthConverter();

            Assert.IsAssignableFrom<IValueConverter>(target);
        }

        [Fact]
        public void Convert_True_Should_Return_ParameterisedWidth()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(true, null, 100d, null);

            // assert
            Assert.Equal(100d, result);
        }

        [Fact]
        public void Convert_False_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(false, null, 100d, null);

            // assert
            Assert.Equal(0d, result);
        }

        [Fact]
        public void Convert_ValueNotBoolean_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert("hello", null, 100d, null);

            // assert
            Assert.Equal(0d, result);
        }

        [Fact]
        public void Convert_ParameterNotDouble_Should_Return_Zero()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act
            var result = target.Convert(false, null, "hello", null);

            // assert
            Assert.Equal(0d, result);
        }

        [Fact]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new ColumnWidthConverter();

            // act - should throw
            Assert.Throws<NotImplementedException>(() => target.ConvertBack(null, null, null, null));
        }
    }
}
