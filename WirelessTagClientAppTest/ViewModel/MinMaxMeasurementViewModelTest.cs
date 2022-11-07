using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        }

        [TestMethod]
        public void Difference_Getter_Should_ReturnDifferenceInTemperatures()
        {
            // act
            var target = new MinMaxMeasurementViewModel();
            target.Minimum.Temperature = -10d;
            target.Maximum.Temperature = 25d;

            // assert
            Assert.AreEqual(35d, target.Difference);
        }
    }
}
