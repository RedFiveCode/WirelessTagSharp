using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the <see cref="TemperatureMeasurement"/> class
    /// </summary>
    [TestClass]
    public class MeasurementViewModelTest
    {
        [TestMethod]
        public void Ctor_ShouldSetProperties()
        {
            // arrange
            var target = new TemperatureMeasurement(0d, new DateTime(2022, 1, 1));

            // act/assert
            Assert.AreEqual(0d, target.Temperature);
            Assert.AreEqual(32, target.TemperatureF);
            Assert.IsFalse(target.IsToday);
        }

        [TestMethod]
        public void IsToday_Today_Should_ReturnTrue()
        {
            // arrange
            var target = new TemperatureMeasurement(20d, DateTime.Now);

            // act/assert
            Assert.IsTrue(target.IsToday);
        }

        [TestMethod]
        public void IsToday_NotToday_Should_ReturnFalse()
        {
            // arrange
            var target = new TemperatureMeasurement(20d, new DateTime(2022, 1, 1));

            // act/assert
            Assert.IsFalse(target.IsToday);
        }
    }
}
