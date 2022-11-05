using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="TimeIntervalValueConverter"/> class
    /// </summary>
    [TestClass]
    public class IntervalValueConverterTest
    {
        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new TimeIntervalValueConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_Null_Should_Return_EmptyString()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void Convert_InvalidType_String_Should_Return_EmptyString()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(42, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void Convert_Today_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Today, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Today", result);
        }

        [TestMethod]
        public void Convert_Yesterday_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Yesterday, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Yesterday", result);
        }

        [TestMethod]
        public void Convert_Last7Days_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Last7Days, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Last 7 days", result);
        }

        [TestMethod]
        public void Convert_Last30Days_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Last30Days, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("Last 30 days", result);
        }

        [TestMethod]
        public void Convert_ThisYear_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.ThisYear, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("This year", result);
        }

        [TestMethod]
        public void Convert_All_String_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.All, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("All", result);
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act - should throw
            var result = target.ConvertBack(null, null, null, null);
        }
    }
}
