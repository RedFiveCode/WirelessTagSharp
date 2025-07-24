using System;
using System.Linq;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;
using Xunit;

namespace WirelessTagClientLib.Test
{
    internal class AssertHelper
    {
        public static void AssertTagInfo(TagInfo actual, string expectedName, string expectedComment, Guid expectedId)
        {
            Assert.NotNull(actual);

            Assert.Equal(expectedName, actual.Name);
            Assert.Equal(expectedComment, actual.Comment);
            Assert.Equal(expectedId, actual.Uuid);

            Assert.NotEqual(DateTime.MinValue, actual.LastCommunication);
            Assert.NotEqual(0, actual.TagType);
        }

        public static void AssertTemperatureInfo(Measurement actual, DateTime expectedTime, double expectedTemperature)
        {
            Assert.NotNull(actual);

            Assert.Equal(actual.Time, expectedTime);

            // is near
            AssertIsNear(actual.Temperature, expectedTemperature, 0.1);
        }

        public static void AssertHourlyReading(HourlyReading actual, DateTime expectedDate)
        {
            Assert.NotNull(actual);

            Assert.Equal(actual.Date, expectedDate);

            Assert.NotNull(actual.Temperatures);
            Assert.Equal(24, actual.Temperatures.Count);

            Assert.NotNull(actual.Humidities);
            Assert.Equal(24, actual.Humidities.Count);

            //AssertDoubleListContains(actual.Humidities, 97d);
        }

        public static bool IsNear(double actual, double expected, double epsilon = 0.1)
        {
            var difference = Math.Abs(actual - expected);
            return difference <= epsilon;
        }

        public static void AssertIsNear(double actual, double expected, double epsilon = 0.1)
        {
            var difference = Math.Abs(actual - expected);
            Assert.True(difference <= epsilon, $"Actual value {actual} does not equal expected value {expected}, within tolerance {epsilon}");
        }
        public static void AssertDoubleListContains(List<double> list, double value)
        {
            Assert.True(list.Any(d => IsNear(d, value)));
        }
    }
}
