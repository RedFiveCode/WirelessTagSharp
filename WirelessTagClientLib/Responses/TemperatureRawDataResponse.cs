using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WirelessTagClientLib.RawResponses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TemperatureRawDataItem
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("temp_degC")]
        public double TempDegC { get; set; }

        [JsonProperty("cap")]
        public double Cap { get; set; }

        [JsonProperty("lux")]
        public int Lux { get; set; }

        [JsonProperty("battery_volts")]
        public double BatteryVolts { get; set; }
    }

    public class TemperatureRawDataResponse
    {
        [JsonProperty("d")]
        public List<TemperatureRawDataItem> Data { get; } = new List<TemperatureRawDataItem>();
    }


}
