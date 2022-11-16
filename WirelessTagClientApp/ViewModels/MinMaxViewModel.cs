using System.Collections.ObjectModel;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Commands;
using System.Windows.Input;

namespace WirelessTagClientApp.ViewModels
{
    public class MinMaxViewModel : ViewModelBase
    {
        private ObservableCollection<MinMaxMeasurementViewModel> data;
        private CopyMinMaxTagsComand copyCommand;

        /// <summary>
        /// ctor
        /// </summary>
        public MinMaxViewModel()
        {
            data = new ObservableCollection<MinMaxMeasurementViewModel>();
            copyCommand = new CopyMinMaxTagsComand();
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
        /// Get the command to copy data to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return copyCommand.Command; }
        }
    }
}
