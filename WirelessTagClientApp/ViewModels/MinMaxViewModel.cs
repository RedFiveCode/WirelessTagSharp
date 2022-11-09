using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WirelessTagClientApp.Common;
using WirelessTagClientLib;
using WirelessTagClientApp.Utils;
using MoreLinq;
using WirelessTagClientLib.DTO;
using System.Windows;
using WirelessTagClientApp.Commands;

namespace WirelessTagClientApp.ViewModels
{
    public class MinMaxViewModel : ViewModelBase
    {
        private ObservableCollection<MinMaxMeasurementViewModel> data;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel()
        {
            data = new ObservableCollection<MinMaxMeasurementViewModel>();
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
    }
}
