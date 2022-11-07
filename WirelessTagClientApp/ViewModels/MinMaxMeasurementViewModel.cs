﻿using System;

namespace WirelessTagClientApp.ViewModels
{
    /// <summary>
    /// Stores minimum and maximum temperature measurement over a period of time 
    /// </summary>
    public class MinMaxMeasurementViewModel
    {
        public MinMaxMeasurementViewModel()
        {
            TagId = -1;
            TagName = String.Empty;
            Interval = TimeInterval.All;
            Minimum = new Measurement();
            Maximum = new Measurement();
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
        public Measurement Minimum { get; set; }

        /// <summary>
        /// Maximum temperature measurement
        /// </summary>
        public Measurement Maximum { get; set; }

        /// <summary>
        /// Get the difference between the maximum and minimum temperatures
        /// </summary>
        public double Difference
        {
            get { return (Minimum != null && Maximum != null ? Maximum.Temperature - Minimum.Temperature : Double.NaN); }
        }
    }

    /// <summary>
    /// Stores a temperature measurement (temperature and timestamp)
    /// </summary>
    public class Measurement
    {
        /// <summary>
        /// Date and Time.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Temperature (degrees C).
        /// </summary>
        public double Temperature { get; set; }
    }
}