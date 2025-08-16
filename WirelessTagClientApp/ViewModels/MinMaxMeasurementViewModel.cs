using System;
using System.Diagnostics;

namespace WirelessTagClientApp.ViewModels
{
    /// <summary>
    /// Stores minimum and maximum temperature measurement over a period of time 
    /// </summary>
    [DebuggerDisplay("Tag={TagId}, Interval={Interval}, Minimum={Minimum}, Maximum={Maximum}")]
    public class MinMaxMeasurementViewModel
    {
        public MinMaxMeasurementViewModel()
        {
            TagId = -1;
            TagName = String.Empty;
            Interval = TimeInterval.All;
            Minimum = new TemperatureMeasurement();
            Maximum = new TemperatureMeasurement();
            Count = -1;
        }

        /// <summary>
        /// Tag Id
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Tag name
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Measurement interval
        /// </summary>
        public TimeInterval Interval { get; set; }

        /// <summary>
        /// Start measurement interval
        /// </summary>
        public DateTime IntervalFrom { get; set; }

        /// <summary>
        /// End measurement interval
        /// </summary>
        public DateTime IntervalTo { get; set; }

        /// <summary>
        /// Minimum temperature measurement
        /// </summary>
        public TemperatureMeasurement Minimum { get; set; }

        /// <summary>
        /// Maximum temperature measurement
        /// </summary>
        public TemperatureMeasurement Maximum { get; set; }

        /// <summary>
        /// Number of temperature measurements in the time interval
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Get the difference between the maximum and minimum temperatures (degrees C)
        /// </summary>
        public double Difference
        {
            get { return (Minimum != null && Maximum != null ? Maximum.Temperature - Minimum.Temperature : Double.NaN); }
        }

        /// <summary>
        /// Get the difference between the maximum and minimum temperatures (degrees F)
        /// </summary>
        public double DifferenceF
        {
            get { return (Minimum != null && Maximum != null ? Maximum.TemperatureF - Minimum.TemperatureF : Double.NaN); }
        }
    }
}
