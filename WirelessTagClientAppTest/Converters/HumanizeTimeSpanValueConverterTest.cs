using Xunit;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="HumanizeTimeSpanValueConverter"/> class
    /// </summary>
    
    public class HumanizeTimeSpanValueConverterTest
    {
        [Fact]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new HumanizeTimeSpanValueConverter();

            Assert.IsAssignableFrom<IValueConverter>(target);
        }

        [Fact]
        public void Convert_DateTime_Should_Return_FriendlyString()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();
            var now = DateTime.Now;

            // act
            var result = target.Convert(now, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.True(result.ToString().EndsWith("ago"));
        }

        [Fact]
        public void Convert_ParameterNotDateTime_Should_Return_EmptyString()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();

            // act
            var result = target.Convert("hello", null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Empty, result);
        }

        [Fact]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();

            // act - should throw
            Assert.Throws<NotImplementedException>(() => target.ConvertBack(null, null, null, null));
        }
    }
}
