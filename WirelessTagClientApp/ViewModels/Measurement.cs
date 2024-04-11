using System;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.ViewModels
{
    /// <summary>
    /// Stores a temperature measurement (temperature and timestamp)
    /// </summary>
    public class Measurement
    {
        public Measurement() : this(0d, DateTime.MinValue) { }

        public Measurement(double temperature, DateTime timestamp)
        {
            Temperature = temperature;
            TemperatureF = TemperatureConvertor.ConvertToFahrenheit(temperature);
            Timestamp = timestamp;
            IsToday = (Timestamp.Date == DateTime.Today);
        }

        /// <summary>
        /// Date and Time.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Temperature (degrees C).
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        /// Temperature (degrees F).
        /// </summary>
        public double TemperatureF { get; private set; }

        /// <summary>
        /// Returns true if the measurement occurred today
        /// </summary>
        public bool IsToday { get; private set; }
    }
}
