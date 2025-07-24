using Xunit;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class ToggleTemperatureUnitsCommandTest
    {
        [Fact]
        public void ToggleViewCommand_Implements_ICommand()
        {
            var target = new ToggleTemperatureUnitsCommand();

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
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
            Assert.Equal(TemperatureUnits.Farenheit, viewModel.TemperatureUnits);
        }

        [Fact]
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
            Assert.Equal(TemperatureUnits.Celsius, viewModel.TemperatureUnits);
        }
    }
}
