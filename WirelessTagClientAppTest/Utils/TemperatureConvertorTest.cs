using Xunit;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    
    public class TemperatureConvertorTest
    {
        [Fact]
        public void Convert_0_Should_Return_32()
        {
            // act
            var result = TemperatureConvertor.ConvertToFahrenheit(0d);

            // assert
            Assert.Equal(32d, result);
        }

        [Fact]
        public void Convert_100_Should_Return_212()
        {
            // act
            var result = TemperatureConvertor.ConvertToFahrenheit(100d);

            // assert
            Assert.Equal(212d, result);
        }
    }
}
