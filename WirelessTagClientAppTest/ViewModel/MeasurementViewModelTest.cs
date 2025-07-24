using Xunit;
using System;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the <see cref="TemperatureMeasurement"/> class
    /// </summary>
    
    public class MeasurementViewModelTest
    {
        [Fact]
        public void Ctor_ShouldSetProperties()
        {
            // arrange
            var target = new TemperatureMeasurement(0d, new DateTime(2022, 1, 1));

            // act/assert
            Assert.Equal(0d, target.Temperature);
            Assert.Equal(32, target.TemperatureF);
            Assert.False(target.IsToday);
        }

        [Fact]
        public void IsToday_Today_Should_ReturnTrue()
        {
            // arrange
            var target = new TemperatureMeasurement(20d, DateTime.Now);

            // act/assert
            Assert.True(target.IsToday);
        }

        [Fact]
        public void IsToday_NotToday_Should_ReturnFalse()
        {
            // arrange
            var target = new TemperatureMeasurement(20d, new DateTime(2022, 1, 1));

            // act/assert
            Assert.False(target.IsToday);
        }
    }
}
