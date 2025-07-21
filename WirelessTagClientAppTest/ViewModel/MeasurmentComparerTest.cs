using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the MeasurmentComparer class
    /// </summary>
    [TestClass]
    public class MeasurmentComparerTest
    {
        [TestMethod]
        public void Equals_SameInstance_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement();

            var result = target.Equals(x, x);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_FirstNull_Returns_False()
        {
            var target = new MeasurmentComparer();
            var y = new Measurement();

            var result = target.Equals(null, y);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_SecondNull_Returns_False()
        {
            var target = new MeasurmentComparer();
            var x= new Measurement();

            var result = target.Equals(x, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_SameTimeValuesSameTemperatureeValues_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 1), 20d);

            var result = target.Equals(x, y);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_SameTimeValuesDifferentTemperatureValues_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 1), 21d);

            var result = target.Equals(x, y);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_DifferentTimeValues_Returns_False()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 2), 20d);

            var result = target.Equals(x, y);

            Assert.IsFalse(result);
        }
    }
}
