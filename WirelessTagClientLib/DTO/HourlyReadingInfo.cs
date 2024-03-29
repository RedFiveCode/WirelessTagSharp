﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WirelessTagClientLib.DTO
{
    [DebuggerDisplay("Readings={HourlyReadings.Count")]
    public class HourlyReadingInfo
    {
        public List<HourlyReading> HourlyReadings { get; internal set; } = new List<HourlyReading>();

        public int TemperatureUnits { get; set; }
    }

    /// <summary>
    /// Hourly temperature and humidity readings for a given day
    /// </summary>
    [DebuggerDisplay("Date={Date}, Temperatures={Temperatures.Count}")]
    public class HourlyReading
    {
        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// Hourly temperatures readings (24)
        /// </summary>
        public List<double> Temperatures { get; internal set; } = new List<double>();

        /// <summary>
        /// Hourly humidity readings (24)
        /// </summary>
        public List<double> Humidities { get; internal set; } = new List<double>();

        //public object TempsBase64 { get; set; }
        //public object CapsBase64 { get; set; }
    }
}
