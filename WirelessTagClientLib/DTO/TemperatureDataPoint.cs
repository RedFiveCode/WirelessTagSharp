using System;
using System.Collections.Generic;

namespace WirelessTagClientLib.DTO
{
    public class TemperatureDataPointList
    {
        public List<TemperatureDataPoint> Data { get; internal set; } = new List<TemperatureDataPoint>();
    }

    public class TemperatureDataPoint
    {
        /// <summary>
        /// Date and Time
        /// </summary>
        public DateTime Time { get; internal set; }

        /// <summary>
        /// Temperature (C)
        /// </summary>
        public double Temperature { get; internal set; }

        /// <summary>
        /// Relative humidity (%)
        /// </summary>
        public double Humidity { get; internal set; }

        /// <summary>
        /// Light
        /// </summary>
        public int Lux { get; internal set; }

        /// <summary>
        /// Battery (V)
        /// </summary>
        public double Battery { get; internal set; }
    }
}
