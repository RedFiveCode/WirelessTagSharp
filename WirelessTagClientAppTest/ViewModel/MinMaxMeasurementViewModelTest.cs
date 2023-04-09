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
                Minimum = new Measurement(-10d, DateTime.MinValue),
                Maximum = new Measurement(25d, DateTime.MinValue)
            };

            // assert
            Assert.AreEqual(35d, target.Difference);
        }
    }

    [TestClass]
    public class MeasurementViewModelTest
    {
        [TestMethod]
        public void IsToday_Today_Should_ReturnTrue()
        {
            // arrange
            var target = new Measurement(20d, DateTime.Now);

            // act/assert
            Assert.IsTrue(target.IsToday);
        }

        [TestMethod]
        public void IsToday_NotToday_Should_ReturnFalse()
        {
            // arrange
            var target = new Measurement(20d, new DateTime(2022, 1, 1));

            // act/assert
            Assert.IsFalse(target.IsToday);
        }
    }
}
