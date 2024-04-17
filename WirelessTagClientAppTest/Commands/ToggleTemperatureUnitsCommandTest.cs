using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    [TestClass]
    public class ToggleTemperatureUnitsCommandTest
    {
        [TestMethod]
        public void ToggleViewCommand_Implements_ICommand()
        {
            var target = new ToggleTemperatureUnitsCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void ToggleViewCommand_Celsius_SetsUnitsToFahrenheit()
        {
            // arrange
            var target = new ToggleTemperatureUnitsCommand();
            var viewModel = new MinMaxViewModel()
            {
                TemperatureUnits = TemperatureUnits.Celsius
            };

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(TemperatureUnits.Farenheit, viewModel.TemperatureUnits);
        }

        [TestMethod]
        public void ToggleViewCommand_Fahrenheit_SetsUnitsToCelsius()
        {
            // arrange
            var target = new ToggleTemperatureUnitsCommand();
            var viewModel = new MinMaxViewModel()
            {
                TemperatureUnits = TemperatureUnits.Farenheit
            };

            // act
            target.Command.Execute(viewModel);

            // assert
            Assert.AreEqual(TemperatureUnits.Celsius, viewModel.TemperatureUnits);
        }
    }
}
