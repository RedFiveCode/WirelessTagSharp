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
        public const int TemperatureTag = 12;
        private const int HumidityTag = 13;

        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Unique id.
        /// </summary>
        /// <remarks>
        /// Image Url is https://www.mytaglist.com/eth/tags/u-u-i-d.jpg
        /// </remarks>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Comment text.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Id number
        /// </summary>
        public int SlaveId { get; set; }

        /// <summary>
        /// Tag type;
        /// 12 = Temperature, 13 = Temperature and Relative Humidity.
        /// </summary>
        public int TagType { get; set; }

        /// <summary>
        /// Date and time of last communication.
        /// </summary>
        public DateTime LastCommunication { get; set; }

        /// <summary>
        /// RF signal strength (dBm).
        /// </summary>
        public int SignalStrength { get; set; }

        /// <summary>
        /// Battery voltage (V).
        /// </summary>
        public double BatteryVoltage { get; set; }

        /// <summary>
        /// Temperature (degrees C).
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Image MD5 hash.
        /// </summary>
        public string ImageMD5 { get; set; }

        /// <summary>
        /// Relative Humidity (%).
        /// </summary>
        public double RelativeHumidity { get; set; }

        /// <summary>
        /// Percentage battery capacity remaining.
        /// </summary>
        public double BatteryRemaining { get; set; }

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
        public bool IsOutOfRange { get; set; }

        // further properties are possible; see TagListData
    }
}
