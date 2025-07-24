using Xunit;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="EmptyStringConverter"/> class.
    /// </summary>
    
    public class EmptyStringConverterTest
    {
        [Fact]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new EmptyStringConverter();

            Assert.IsAssignableFrom<IValueConverter>(target);
        }

        [Fact]
        public void Convert_Null_Should_Return_none()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal("(none)", result);
        }

        [Fact]
        public void Convert_Empty_String_Should_Return_none()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert("", null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal("(none)", result);
        }

        [Fact]
        public void Convert_Non_Empty_String_Should_Return_Original_String()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert("hello", null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal("hello", result);
        }

        [Fact]
        public void Convert_Null_Should_Return_UnavailableText()
        {
            // arrange
            var target = new EmptyStringConverter()
            {
                UnavailableText = "Value not available"
            };

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal("Value not available", result);
        }
    }
}
