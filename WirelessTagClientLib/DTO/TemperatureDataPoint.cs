using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WirelessTagClientLib.DTO
{
    [DebuggerDisplay("Data={Data.Count}")]
    public class TemperatureDataPointList
    {
        public List<TemperatureDataPoint> Data { get; internal set; } = new List<TemperatureDataPoint>();
    }

    [DebuggerDisplay("Time={Time}, Temperature={Temperature}, Humidity={Humidity}, Lux={Lux}, Battery={Battery}")]
    public class TemperatureDataPoint
    {
        public TemperatureDataPoint()
        { }

        public TemperatureDataPoint(DateTime time, double temperature)
        {
            Time = time;
            Temperature = temperature;
        }

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

    [DebuggerDisplay("Tag={TagId}, Time={Time}, Temperature={Temperature}, Humidity={Humidity}, Lux={Lux}, Battery={Battery}")]
    public class TagMeasurementDataPoint
    {
        public TagMeasurementDataPoint()
        { }

        public TagMeasurementDataPoint(int tagId, DateTime time, double temperature, double humidity = -1d, int lux = -1, double battery = -1d)
        {
            TagId = tagId;
            Time = time;
            Temperature = temperature;
        }

        /// <summary>
        /// Tag Id
        /// </summary>
        public int TagId { get; internal set; }

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
