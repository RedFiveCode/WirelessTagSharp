using System;
using System.Linq;
using Xunit;

namespace WirelessTagClientLib.Test.Utils
{
    public class DateTimeChunkerTest
    {
        [Fact]
        public void SplitDateTimeRange_From_After_To_ThrowsArgumentException()
        {
            // arrange
            var from = DateTime.Now;
            var to = from.AddDays(-10);

            // act; should throw
            Assert.Throws<ArgumentException>(() => DateTimeChunker.SplitDateTimeRange(from, to, TimeSpan.FromDays(1)));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SplitDateTimeRange_Span_LessThanZero_ThrowsArgumentOutOfRangeException(int span)
        {
            // arrange
            var from = DateTime.Now;
            var to = from.AddDays(10);

            // act; should throw
            Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeChunker.SplitDateTimeRange(from, to, TimeSpan.FromDays(span)));
        }

        [Fact]
        public void SplitDateTimeRange_Wide_Returns_ExpectedItems()
        {
            // arrange
            var from = new DateTime(2025, 1, 1, 10, 11, 12);
            var to = new DateTime(2025, 1, 12, 10, 11, 12);

            // act
            var results =  DateTimeChunker.SplitDateTimeRange(from, to, TimeSpan.FromDays(5));

            // assert
            Assert.NotNull(results);    
            Assert.Equal(3, results.Count); // 2 chunks of 5 days each plus remainder

            var start = new DateTime(2025, 1, 1);
            AssertDateTimeRange(results[0], new DateTime(2025, 1, 1), new DateTime(2025, 1, 5, 23, 59, 59).AddTicks(9999999L));
            AssertDateTimeRange(results[1], new DateTime(2025, 1, 6), new DateTime(2025, 1, 10, 23, 59, 59).AddTicks(9999999L));
            AssertDateTimeRange(results[2], new DateTime(2025, 1, 11), new DateTime(2025, 1, 12, 23, 59, 59).AddTicks(9999999L));
        }

        [Fact]
        public void SplitDateTimeRange_Narrow_Returns_ExpectedItems()
        {
            // arrange
            var from = new DateTime(2025, 1, 1, 10, 11, 12);
            var to = new DateTime(2025, 1, 3, 10, 11, 12);

            // act
            var results = DateTimeChunker.SplitDateTimeRange(from, to, TimeSpan.FromDays(5));

            // assert
            Assert.NotNull(results);
            Assert.Single(results); // only one chunk since the range is less than 5 days

            var start = new DateTime(2025, 1, 1);
            AssertDateTimeRange(results[0], new DateTime(2025, 1, 1), new DateTime(2025, 1, 3, 23, 59, 59).AddTicks(9999999L));
        }

        private void AssertDateTimeRange((DateTime Start, DateTime End) result, DateTime expectedStart, DateTime expectedEnd)
        {
            Assert.Equal(expectedStart, result.Start);
            Assert.Equal(expectedEnd, result.End);
        }
    }
}
