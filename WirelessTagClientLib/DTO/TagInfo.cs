using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace WirelessTagClientLib.DTO
{
    /// <summary>
    /// Strongly typed class storing result of GetTagList;
    /// Model storing tag information including current temperature, battery, etc
    /// </summary>
    [DebuggerDisplay("Tag={Name}, Type={TagType}, Id={Uuid}")]
    public class TagInfo
    {
        private const int TemperatureTag = 12;
        private const int HumidityTag = 13;

        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Unique id.
        /// </summary>
        /// <remarks>
        /// Image Url is https://www.mytaglist.com/eth/tags/u-u-i-d.jpg
        /// </remarks>
        public Guid Uuid { get; internal set; }

        /// <summary>
        /// Comment text.
        /// </summary>
        public string Comment { get; internal set; }

        /// <summary>
        /// Id number
        /// </summary>
        public int SlaveId { get; internal set; }

        /// <summary>
        /// Tag type;
        /// 12 = Temperature, 13 = Temperature and Relative Humidity.
        /// </summary>
        public int TagType { get; internal set; }

        /// <summary>
        /// Date and time of last communication.
        /// </summary>
        public DateTime LastCommunication { get; internal set; }

        /// <summary>
        /// RF signal strength (dBm).
        /// </summary>
        public int SignalStrength { get; internal set; }

        /// <summary>
        /// Battery voltage (V).
        /// </summary>
        public double BatteryVoltage { get; internal set; }

        /// <summary>
        /// Temperature (degrees C).
        /// </summary>
        public double Temperature { get; internal set; }

        /// <summary>
        /// Image MD5 hash.
        /// </summary>
        public string ImageMD5 { get; internal set; }

        /// <summary>
        /// Relative Humidity (%).
        /// </summary>
        public double RelativeHumidity { get; internal set; }

        /// <summary>
        /// Percentage battery capacity remaining.
        /// </summary>
        public double BatteryRemaining { get; internal set; }

        /// <summary>
        /// Returns true if the tag measures temperature.
        /// </summary>
        /// <remarks>
        /// Humidity tags also measure temperature.
        /// </remarks>
        public bool IsTemperatureTag
        {
            get { return TagType == TemperatureTag || TagType == HumidityTag; }
        }

        /// <summary>
        /// Returns true if the tag measures relative humidity.
        /// </summary>
        public bool IsHumidityTag
        {
            get { return TagType == HumidityTag; }
        }

        /// <summary>
        /// Out of range flag.
        /// </summary>
        public bool IsOutOfRange { get; internal set; }

        // further properties are possible; see TagListData
    }
}
