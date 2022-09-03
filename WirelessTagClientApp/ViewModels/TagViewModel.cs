using System;
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
        private ViewMode mode;
        private string name;
        private string description;
        private int id;
        private Guid uuid;
        private DateTime lastCommunication;
        private int signalStrength;
        private double temperature;
        private double relativeHumidity;
        private double batteryVoltage;
        private double batteryRemaining;

        public enum ViewMode { Temperature = 0, TemperatureF, Humidity, BatteryVoltage, BatteryPercent }

        public TagViewModel()
        {
            Name = String.Empty;
            Description = String.Empty;
            Mode = ViewMode.Temperature;
        }

        /// <summary>
        /// Get/set the view mode (temperature, humidity, etc)
        /// </summary>
        public ViewMode Mode
        {
            get { return mode; }

            set
            {
                mode = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the tag name
        /// </summary>
        public string Name
        {
            get { return name; }

            set
            {
                name = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the tag description text
        /// </summary>
        public string Description
        {
            get { return description; }

            set
            {
                description = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the tag id
        /// </summary>
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the unique id
        /// </summary>
        public Guid Uuid
        {
            get { return uuid; }
            set
            {
                uuid = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the temperature (degrees Celsius)
        /// </summary>
        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("TemperatureFahrenheit");
            }
        }

        /// <summary>
        /// Get/set the temperature (degrees Fahrenheit)
        /// </summary>
        public double TemperatureFahrenheit
        {
            get { return TemperatureConvertor.ConvertToFarenheit(temperature); }
        }

        /// <summary>
        /// Get/set the relative humidity (%)
        /// </summary>
        public double RelativeHumidity
        {
            get { return relativeHumidity; }
            set
            {
                relativeHumidity = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the date/time of last communication with the tag
        /// </summary>
        public DateTime LastCommunication
        {
            get { return lastCommunication; }
            set
            {
                lastCommunication = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the signal strength (dBm)
        /// </summary>
        public int SignalStrength
        {
            get { return signalStrength; }
            set
            {
                signalStrength = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the battery voltage (V).
        /// </summary>
        public double BatteryVoltage
        {
            get { return batteryVoltage; }
            set
            {
                batteryVoltage = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the Percentage battery capacity remaining.
        /// </summary>
        public double BatteryRemaining
        {
            get { return batteryRemaining; }
            set
            {
                batteryRemaining = value;
                NotifyPropertyChanged();
            }
        }
    }
}
