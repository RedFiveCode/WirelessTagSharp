using System.Collections.ObjectModel;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Commands;
using System.Windows.Input;
using System;
using System.Threading.Tasks;
using WirelessTagClientLib;

namespace WirelessTagClientApp.ViewModels
{
    public enum TemperatureUnits { Celsius = 0, Farenheit = 1 }

    public class MinMaxViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _parentViewModel;
        private readonly CopyMinMaxTagsCommand _copyCommand;
        private readonly CopyMinMaxTagsCommand _copyRawDataCommand;
        private readonly ToggleTemperatureUnitsCommand _toggleUnitsCommand;
        private readonly RefreshMinMaxTagsCommand _refreshCommand;
        private readonly TemperatureRawDataCache _rawDataCache;

        private ObservableCollection<MinMaxMeasurementViewModel> _data;
        private DateTime _lastUpdated;
        private TemperatureUnits _temperatureUnits;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel(MainWindowViewModel parentViewModel = null, ICacheFileReaderWriter cacheReaderWrite = null)
        {
            _parentViewModel = parentViewModel;

            _data = new ObservableCollection<MinMaxMeasurementViewModel>();
            _lastUpdated = DateTime.MinValue;
            _copyCommand = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);
            _copyRawDataCommand = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.RawData);
            _toggleUnitsCommand = new ToggleTemperatureUnitsCommand();
            _refreshCommand = new RefreshMinMaxTagsCommand(parentViewModel?.Client, cacheReaderWrite, parentViewModel?.Options);

            _rawDataCache = new TemperatureRawDataCache();
            _temperatureUnits = TemperatureUnits.Celsius;
        }

        public async Task RefreshAsync()
        {
            await _refreshCommand.ExecuteAsync(this);
        }

        /// <summary>
        /// Get/set the list of temperature minimum and maximum measurements
        /// </summary>
        public ObservableCollection<MinMaxMeasurementViewModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the time last updated
        /// </summary>
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                _lastUpdated = value;
                NotifyPropertyChanged();
            }
        }

        public TemperatureUnits TemperatureUnits
        {
            get { return _temperatureUnits; }
            set
            {
                _temperatureUnits = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsTemperatureCelsius");
                NotifyPropertyChanged("IsTemperatureFahrenheit");
            }
        }

        public bool IsTemperatureCelsius
        {
            get { return _temperatureUnits == TemperatureUnits.Celsius; }
        }

        public bool IsTemperatureFahrenheit
        {
            get { return _temperatureUnits == TemperatureUnits.Farenheit; }
        }

        /// <summary>
        /// Get the cache of raw measurements
        /// </summary>
        public TemperatureRawDataCache RawDataCache
        {
            get { return _rawDataCache; }
        }

        /// <summary>
        /// Get the command to copy data to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return _copyCommand.Command; }
        }

        /// <summary>
        /// Get the command to copy raw data to the clipboard
        /// </summary>
        public ICommand CopyRawDataCommand
        {
            get { return _copyRawDataCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the temperature units
        /// </summary>
        public ICommand ToggleTemperatureUnitsCommand
        {
            get { return _toggleUnitsCommand.Command; }
        }

        /// <summary>
        /// Get the command to refresh the view
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return _refreshCommand.Command; }
        }

        /// <summary>
        /// Get the parent view-model (for command routing)
        /// </summary>
        public MainWindowViewModel ParentViewModel
        {
            get { return _parentViewModel; }
        }
    }
}
