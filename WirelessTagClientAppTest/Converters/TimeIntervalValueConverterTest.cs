using Xunit;
using System;
using System.Windows.Data;
using WirelessTagClientApp.Converters;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="TimeIntervalValueConverter"/> class
    /// </summary>
    
    public class TimeIntervalValueConverterTest
    {
        [Fact]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new TimeIntervalValueConverter();

            Assert.IsAssignableFrom<IValueConverter>(target);
        }

        [Fact]
        public void Convert_Null_Should_Return_EmptyString()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal(String.Empty, result);
        }

        [Fact]
        public void Convert_InvalidType_Should_Return_EmptyString()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(42, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);

            Assert.Equal(String.Empty, result);
        }

        [Fact]
        public void Convert_Today_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Today, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal("Today", result);
        }

        [Fact]
        public void Convert_Yesterday_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Yesterday, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal("Yesterday", result);
        }

        [Fact]
        public void Convert_Last7Days_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Last7Days, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal("Last 7 days", result);
        }

        [Fact]
        public void Convert_Last30Days_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.Last30Days, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal("Last 30 days", result);
        }

        [Fact]
        public void Convert_ThisYear_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.ThisYear, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("This year ({0})", currentYear), result);
        }

        [Fact]
        public void Convert_January_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.January , null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("January {0}", currentYear), result);
        }

        [Fact]
        public void Convert_February_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.February, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("February {0}", currentYear), result);
        }

        [Fact]
        public void Convert_March_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.March, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("March {0}", currentYear), result);
        }

        [Fact]
        public void Convert_April_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.April, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("April {0}", currentYear), result);
        }

        [Fact]
        public void Convert_May_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.May, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("May {0}", currentYear), result);
        }

        [Fact]
        public void Convert_June_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.June, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("June {0}", currentYear), result);
        }

        [Fact]
        public void Convert_July_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.July, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("July {0}", currentYear), result);
        }

        [Fact]
        public void Convert_August_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.August, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("August {0}", currentYear), result);
        }

        [Fact]
        public void Convert_September_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.September, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("September {0}", currentYear), result);
        }

        [Fact]
        public void Convert_October_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.October, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("October {0}", currentYear), result);
        }

        [Fact]
        public void Convert_November_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.November, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("November {0}", currentYear), result);
        }

        [Fact]
        public void Convert_December_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.December, null, null, null);

            // assert
            var currentYear = DateTime.Now.Year;
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(String.Format("December {0}", currentYear), result);
        }

        [Fact]
        public void Convert_All_Should_Return_ExpectedText()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act
            var result = target.Convert(TimeInterval.All, null, null, null);

            // assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal("All", result);
        }

        [Fact]
        public void ConvertBack_Should_Throw_NotImplementedException()
        {
            // arrange
            var target = new TimeIntervalValueConverter();

            // act - should throw
            Assert.Throws<NotImplementedException>(() => target.ConvertBack(null, null, null, null));
        }
    }
}
