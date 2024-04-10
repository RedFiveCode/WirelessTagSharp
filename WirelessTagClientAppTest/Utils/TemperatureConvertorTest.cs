using Microsoft.VisualStudio.TestTools.UnitTesting;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    [TestClass]
    public class TemperatureConvertorTest
    {
        [TestMethod]
        public void Convert_0_Should_Return_32()
        {
            // act
            var result = TemperatureConvertor.ConvertToFahrenheit(0d);

            // assert
            Assert.AreEqual(32d, result);
        }

        [TestMethod]
        public void Convert_100_Should_Return_212()
        {
            // act
            var result = TemperatureConvertor.ConvertToFahrenheit(100d);

            // assert
            Assert.AreEqual(212d, result);
        }
    }
}
