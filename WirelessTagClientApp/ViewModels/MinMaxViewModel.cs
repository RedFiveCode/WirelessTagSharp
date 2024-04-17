using System.Collections.ObjectModel;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Commands;
using System.Windows.Input;
using System;

namespace WirelessTagClientApp.ViewModels
{
    public class MinMaxViewModel : ViewModelBase
    {
        private ObservableCollection<MinMaxMeasurementViewModel> data;
        private DateTime lastUpdated;
        private CopyMinMaxTagsComand copyCommand;
        private TemperatureRawDataCache rawDataCache;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel()
        {
            data = new ObservableCollection<MinMaxMeasurementViewModel>();
            lastUpdated = DateTime.MinValue;
            copyCommand = new CopyMinMaxTagsCommand();
            rawDataCache = new TemperatureRawDataCache();
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
    }
}
