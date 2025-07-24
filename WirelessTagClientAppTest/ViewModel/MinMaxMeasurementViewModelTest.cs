using Xunit;
using System;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the <see cref="MinMaxMeasurementViewModel"/> class
    /// </summary>
    
    public class MinMaxMeasurementViewModelTest
    {
        [Fact]
        public void Ctor_Should_Initialise_Properties_To_Expected_Values()
        {
            // act
            var target = new MinMaxMeasurementViewModel();

            // assert
            Assert.Equal(-1, target.TagId);
           Assert.NotNull(target.Minimum);
           Assert.NotNull(target.Maximum);
            Assert.Equal(-1, target.Count);
        }

        [Fact]
        public void Difference_Getter_Should_ReturnDifferenceInTemperatures()
        {
            // act
            var target = new MinMaxMeasurementViewModel()
            {
                Minimum = new TemperatureMeasurement(-10d, DateTime.MinValue),
                Maximum = new TemperatureMeasurement(25d, DateTime.MinValue)
            };

            // assert
            Assert.Equal(35d, target.Difference);
        }

        [Fact]
        public void DifferenceF_Getter_Should_ReturnDifferenceInTemperatures()
        {
            // act
            var target = new MinMaxMeasurementViewModel()
            {
                Minimum = new TemperatureMeasurement(0d, DateTime.MinValue), // 32 F
                Maximum = new TemperatureMeasurement(100d, DateTime.MinValue) // 212 F
            };

            // assert
            Assert.Equal(180d, target.DifferenceF);
        }
    }
}
