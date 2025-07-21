using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib.Test
{
    internal class AssertHelper
    {
        public static void AssertTagInfo(TagInfo actual, string expectedName, string expectedComment, Guid expectedId)
        {
            Assert.IsNotNull(actual);

            Assert.AreEqual(expectedName, actual.Name);
            Assert.AreEqual(expectedComment, actual.Comment);
            Assert.AreEqual(expectedId, actual.Uuid);

            Assert.AreNotEqual(DateTime.MinValue, actual.LastCommunication);
            Assert.AreNotEqual(0, actual.TagType);
        }

        public static void AssertTemperatureInfo(Measurement actual, DateTime expectedTime, double expectedTemperature)
        {
            Assert.IsNotNull(actual);

            Assert.AreEqual(actual.Time, expectedTime);

            // is near
            AssertIsNear(actual.Temperature, expectedTemperature, 0.1);
        }

        public static void AssertHourlyReading(HourlyReading actual, DateTime expectedDate)
        {
            Assert.IsNotNull(actual);

            Assert.AreEqual(actual.Date, expectedDate);

            Assert.IsNotNull(actual.Temperatures);
            Assert.AreEqual(24, actual.Temperatures.Count);

            Assert.IsNotNull(actual.Humidities);
            Assert.AreEqual(24, actual.Humidities.Count);

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
            Assert.IsNotNull(difference <= epsilon, $"Actual value {actual} does not equal expected value {expected}, within tolerance {epsilon}");
        }
        public static void AssertDoubleListContains(List<double> list, double value)
        {
            Assert.IsTrue(list.Any(d => IsNear(d, value)));
        }
    }
}
