using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    public class ToggleTemperatureUnitsCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleTemperatureUnitsCommand"/>
        /// </summary>
        public ToggleTemperatureUnitsCommand()
        {
            Command = new RelayCommandT<MinMaxViewModel>(p => ChangeUnits(p), p => CanChangeUnits(p));
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        private bool CanChangeUnits(MinMaxViewModel viewModel)
        {
            return true; // can always change the units
        }

        private void ChangeUnits(MinMaxViewModel viewModel)
        {
            if (CanChangeUnits(viewModel))
            {
                viewModel.TemperatureUnits = viewModel.IsTemperatureCelsius ? TemperatureUnits.Farenheit : TemperatureUnits.Celsius;
            }
        }
    }
}
