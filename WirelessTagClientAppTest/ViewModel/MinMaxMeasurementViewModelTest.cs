using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the <see cref="MinMaxMeasurementViewModel"/> class
    /// </summary>
    [TestClass]
    public class MinMaxMeasurementViewModelTest
    {
        [TestMethod]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new MinMaxMeasurementViewModel();

            // assert
            Assert.AreEqual(-1, target.TagId);
            Assert.IsNotNull(target.Minimum);
            Assert.IsNotNull(target.Maximum);
            Assert.AreEqual(-1, target.Count);
        }

        [TestMethod]
        public void Difference_Getter_Should_ReturnDifferenceInTemperatures()
        {
            // act
            var target = new MinMaxMeasurementViewModel()
            {
                Minimum = new TemperatureMeasurement(-10d, DateTime.MinValue),
                Maximum = new TemperatureMeasurement(25d, DateTime.MinValue)
            };

            // assert
            Assert.AreEqual(35d, target.Difference);
        }

        [TestMethod]
        public void DifferenceF_Getter_Should_ReturnDifferenceInTemperatures()
        {
            // act
            var target = new MinMaxMeasurementViewModel()
            {
                Minimum = new TemperatureMeasurement(0d, DateTime.MinValue), // 32 F
                Maximum = new TemperatureMeasurement(100d, DateTime.MinValue) // 212 F
            };

            // assert
            Assert.AreEqual(180d, target.DifferenceF);
        }
    }
}
