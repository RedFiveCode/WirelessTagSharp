using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientApp.Test.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="HumanizeTimeSpanValueConverter"/> class
    /// </summary>
    [TestClass]
    public class HumanizeTimeSpanValueConverterTest
    {
        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new HumanizeTimeSpanValueConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_DateTime_Should_Return_FriendlyString()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();
            var now = DateTime.Now;

            // act
            var result = target.Convert(now, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.IsTrue(result.ToString().EndsWith("ago"));
        }

        [TestMethod]
        public void Convert_ParameterNotDateTime_Should_Return_EmptyString()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();

            // act
            var result = target.Convert("hello", null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(String.Empty, result);
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new HumanizeTimeSpanValueConverter();

            // act - should throw
            var result = target.ConvertBack(null, null, null, null);
        }
    }
}
