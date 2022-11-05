using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    [TestClass]
    public class TimeIntervalHelperTest
    {
        [TestMethod]
        public void GetTimeRange_Today_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.Today);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 10, 30), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_Yesterday_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.Yesterday);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 10, 29), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 29, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_Last7Days_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.Last7Days);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 10, 23), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_Last30Days_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.Last30Days);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 9, 30), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_ThisYear_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.ThisYear);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 1, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_AllDays_Returns_ExpectedRange()
        {
            var now = DateTime.Now;

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.All);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.MinValue, result.Item1); // from
            Assert.AreEqual(now, result.Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTimeRange_NotEnumValue_ThrowsException()
        {
            var now = DateTime.Now;

            // act - should throw
            var result = TimeIntervalHelper.GetTimeRange(now, (TimeInterval)99);
        }
    }
}
