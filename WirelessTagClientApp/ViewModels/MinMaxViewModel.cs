using System.Collections.ObjectModel;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Commands;
using System.Windows.Input;
using System;
using System.Threading.Tasks;

namespace WirelessTagClientApp.ViewModels
{
    public enum TemperatureUnits { Celsius = 0, Farenheit = 1 }

    public class MinMaxViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel parentViewModel;

        private ObservableCollection<MinMaxMeasurementViewModel> data;
        private DateTime lastUpdated;
        private CopyMinMaxTagsCommand copyCommand;
        private CopyMinMaxTagsCommand copyRawDataCommand;
        private ToggleTemperatureUnitsCommand toggleUnitsCommand;
        private readonly RefreshMinMaxTagsCommand refreshCommand;
        private TemperatureRawDataCache rawDataCache;
        private TemperatureUnits temperatureUnits;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel(MainWindowViewModel parentViewModel = null)
        {
            this.parentViewModel = parentViewModel;

            data = new ObservableCollection<MinMaxMeasurementViewModel>();
            lastUpdated = DateTime.MinValue;
            copyCommand = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);
            copyRawDataCommand = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.RawData);
            toggleUnitsCommand = new ToggleTemperatureUnitsCommand();
            refreshCommand = new RefreshMinMaxTagsCommand(parentViewModel?.Client, parentViewModel?.Options);

            rawDataCache = new TemperatureRawDataCache();
            temperatureUnits = TemperatureUnits.Celsius;
        }

        public async Task RefreshAsync()
        {
            await refreshCommand.ExecuteAsync(this);
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
        /// Get the command to copy raw data to the clipboard
        /// </summary>
        public ICommand CopyRawDataCommand
        {
            get { return copyRawDataCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the temperature units
        /// </summary>
        public ICommand ToggleTemperatureUnitsCommand
        {
            get { return toggleUnitsCommand.Command; }
        }

        /// <summary>
        /// Get the command to refresh the view
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return refreshCommand.Command; }
        }

        /// <summary>
        /// Get the parent view-model (for command routing)
        /// </summary>
        public MainWindowViewModel ParentViewModel
        {
            get { return parentViewModel; }
        }
    }
}
