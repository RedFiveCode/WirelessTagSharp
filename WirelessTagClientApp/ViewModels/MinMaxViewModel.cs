using System.Collections.ObjectModel;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Commands;
using System.Windows.Input;
using System;

namespace WirelessTagClientApp.ViewModels
{
    public enum TemperatureUnits { Celsius = 0, Farenheit = 1 }

    public class MinMaxViewModel : ViewModelBase
    {
        private ObservableCollection<MinMaxMeasurementViewModel> data;
        private DateTime lastUpdated;
        private CopyMinMaxTagsCommand copyCommand;
        private ToggleTemperatureUnitsCommand toggleUnitsCommand;
        private TemperatureRawDataCache rawDataCache;
        private TemperatureUnits temperatureUnits;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel()
        {
            data = new ObservableCollection<MinMaxMeasurementViewModel>();
            lastUpdated = DateTime.MinValue;
            copyCommand = new CopyMinMaxTagsCommand();
            toggleUnitsCommand = new ToggleTemperatureUnitsCommand();
            rawDataCache = new TemperatureRawDataCache();
            temperatureUnits = TemperatureUnits.Celsius;
        }

        /// <summary>
        /// Get/set the list of temperature minimum and maximum measurements
        /// </summary>
        public ObservableCollection<MinMaxMeasurementViewModel> Data
        {
            get { return data; }
            set
            {
                data = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the time last updated
        /// </summary>
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
                NotifyPropertyChanged();
            }
        }

        public TemperatureUnits TemperatureUnits
        {
            get { return temperatureUnits; }
            set
            {
                temperatureUnits = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsTemperatureCelsius");
                NotifyPropertyChanged("IsTemperatureFahrenheit");
            }
        }

        public bool IsTemperatureCelsius
        {
            get { return temperatureUnits == TemperatureUnits.Celsius; }
        }

        public bool IsTemperatureFahrenheit
        {
            get { return temperatureUnits == TemperatureUnits.Farenheit; }
        }

        /// <summary>
        /// Get the cache of raw measurements
        /// </summary>
        public TemperatureRawDataCache RawDataCache
        {
            get { return rawDataCache; }
        }

        /// <summary>
        /// Get the command to copy data to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return copyCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the temperature units
        /// </summary>
        public ICommand ToggleTemperatureUnitsCommand
        {
            get { return toggleUnitsCommand.Command; }
        }
    }
}
