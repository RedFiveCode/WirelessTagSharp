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
        public void GetTimeRange_January_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.January);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 1, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 1, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_February_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.February);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 2, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 2, 28, 23, 59, 59), result.Item2); // 2022 is not a leap year
        }

        [TestMethod]
        public void GetTimeRange_March_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.March);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 3, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 3, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_April_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.April);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 4, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 4, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_May_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.May);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 5, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 5, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_June_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.June);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 6, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 6, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_July_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.July);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 7, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 7, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_August_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.August);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 8, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 8, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_September_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.September);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 9, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 9, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_October_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.October);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 10, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 10, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_November_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.November);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 11, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 11, 30, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_December_Returns_ExpectedRange()
        {
            var now = new DateTime(2022, 10, 30, 15, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.December);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2022, 12, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2022, 12, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_December_NotCurrentYear_Returns_ExpectedRange()
        {
            var now = new DateTime(2021, 10, 30, 0, 0, 0);

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.December);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2021, 12, 1), result.Item1); // from
            Assert.AreEqual(new DateTime(2021, 12, 31, 23, 59, 59), result.Item2);
        }

        [TestMethod]
        public void GetTimeRange_AllDays_Returns_ExpectedRange()
        {
            var now = DateTime.Now;

            // act
            var result = TimeIntervalHelper.GetTimeRange(now, TimeInterval.All);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(new DateTime(2000, 1, 1), result.Item1); // from
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
