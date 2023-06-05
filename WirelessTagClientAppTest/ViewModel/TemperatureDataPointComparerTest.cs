using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the TemperatureDataPointComparer class
    /// </summary>
    [TestClass]
    public class TemperatureDataPointComparerTest
    {
        [TestMethod]
        public void Equals_SameInstance_Returns_True()
        {
            var target = new TemperatureDataPointComparer();
            var x = new TemperatureDataPoint();

            var result = target.Equals(x, x);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_FirstNull_Returns_False()
        {
            var target = new TemperatureDataPointComparer();
            var y = new TemperatureDataPoint();

            var result = target.Equals(null, y);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_SecondNull_Returns_False()
        {
            var target = new TemperatureDataPointComparer();
            var x= new TemperatureDataPoint();

            var result = target.Equals(x, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_SameTimeValuesSameTemperatureeValues_Returns_True()
        {
            var target = new TemperatureDataPointComparer();
            var x = new TemperatureDataPoint(new DateTime(2023, 1, 1), 20d);
            var y = new TemperatureDataPoint(new DateTime(2023, 1, 1), 20d);

            var result = target.Equals(x, y);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_SameTimeValuesDifferentTemperatureValues_Returns_True()
        {
            var target = new TemperatureDataPointComparer();
            var x = new TemperatureDataPoint(new DateTime(2023, 1, 1), 20d);
            var y = new TemperatureDataPoint(new DateTime(2023, 1, 1), 21d);

            var result = target.Equals(x, y);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_DifferentTimeValues_Returns_False()
        {
            var target = new TemperatureDataPointComparer();
            var x = new TemperatureDataPoint(new DateTime(2023, 1, 1), 20d);
            var y = new TemperatureDataPoint(new DateTime(2023, 1, 2), 20d);

            var result = target.Equals(x, y);

            Assert.IsFalse(result);
        }
    }
}
