using System.Diagnostics;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.ViewModels
{
    /// <summary>
    /// View model for tag summary info.
    /// </summary>
    [DebuggerDisplay("Tag={Name}, Temperature={Temperature}")]
    public class TagViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private int _id;
        private Guid _uuid;
        private DateTime _lastCommunication;
        private int _signalStrength;
        private double _temperature;
        private double _relativeHumidity;
        private double _batteryVoltage;
        private double _batteryRemaining;
        private bool _isHumidityTag;

        public TagViewModel()
        {
            Name = String.Empty;
            Description = String.Empty;
        }

        /// <summary>
        /// Get/set the tag name
        /// </summary>
        public string Name
        {
            get { return _name; }

            set
            {
                _name = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the tag description text
        /// </summary>
        public string Description
        {
            get { return _description; }

            set
            {
                _description = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the tag id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the unique id
        /// </summary>
        public Guid Uuid
        {
            get { return _uuid; }
            set
            {
                _uuid = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the temperature (degrees Celsius)
        /// </summary>
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("TemperatureFahrenheit");
            }
        }

        /// <summary>
        /// Get/set the temperature (degrees Fahrenheit)
        /// </summary>
        public double TemperatureFahrenheit
        {
            get { return TemperatureConvertor.ConvertToFahrenheit(_temperature); }
        }

        /// <summary>
        /// Get/set the relative humidity (%)
        /// </summary>
        public double RelativeHumidity
        {
            get { return _relativeHumidity; }
            set
            {
                _relativeHumidity = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the date/time of last communication with the tag
        /// </summary>
        public DateTime LastCommunication
        {
            get { return _lastCommunication; }
            set
            {
                _lastCommunication = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the signal strength (dBm)
        /// </summary>
        public int SignalStrength
        {
            get { return _signalStrength; }
            set
            {
                _signalStrength = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the battery voltage (V).
        /// </summary>
        public double BatteryVoltage
        {
            get { return _batteryVoltage; }
            set
            {
                _batteryVoltage = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the Percentage battery capacity remaining.
        /// </summary>
        public double BatteryRemaining
        {
            get { return _batteryRemaining; }
            set
            {
                _batteryRemaining = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the flag indicating if the tag supports humidity measurement
        /// </summary>
        public bool IsHumidityTag
        {
            get { return _isHumidityTag; }
            set
            {
                _isHumidityTag = value;
                NotifyPropertyChanged();
            }
        }
    }
}
