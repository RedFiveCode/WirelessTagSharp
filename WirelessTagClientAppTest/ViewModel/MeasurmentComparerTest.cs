using Xunit;
using System;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the MeasurmentComparer class
    /// </summary>
    
    public class MeasurmentComparerTest
    {
        [Fact]
        public void Equals_SameInstance_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement();

            var result = target.Equals(x, x);

            Assert.True(result);
        }

        [Fact]
        public void Equals_FirstNull_Returns_False()
        {
            var target = new MeasurmentComparer();
            var y = new Measurement();

            var result = target.Equals(null, y);

            Assert.False(result);
        }

        [Fact]
        public void Equals_SecondNull_Returns_False()
        {
            var target = new MeasurmentComparer();
            var x= new Measurement();

            var result = target.Equals(x, null);

            Assert.False(result);
        }

        [Fact]
        public void Equals_SameTimeValuesSameTemperatureeValues_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 1), 20d);

            var result = target.Equals(x, y);

            Assert.True(result);
        }

        [Fact]
        public void Equals_SameTimeValuesDifferentTemperatureValues_Returns_True()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 1), 21d);

            var result = target.Equals(x, y);

            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentTimeValues_Returns_False()
        {
            var target = new MeasurmentComparer();
            var x = new Measurement(new DateTime(2023, 1, 1), 20d);
            var y = new Measurement(new DateTime(2023, 1, 2), 20d);

            var result = target.Equals(x, y);

            Assert.False(result);
        }
    }
}
