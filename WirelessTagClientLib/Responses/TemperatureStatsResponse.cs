using Newtonsoft.Json;
using System.Collections.Generic;

namespace WirelessTagClientLib.RawResponses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TemperatureStatsData
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("temps")]
        public List<Temp> Temps { get; } = new List<Temp>();

        [JsonProperty("temp_unit")]
        public int TempUnit { get; set; }
    }

    public class TemperatureStatsResponse
    {
        [JsonProperty("d")]
        public TemperatureStatsData Data { get; set; }
    }

    public class Temp // hourly temperature and humidity readings for a given day
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("temps")] // temperatures per hour (24)
        public List<double> Temps { get; } = new List<double>();

        [JsonProperty("temps_base64")]
        public object TempsBase64 { get; set; }

        [JsonProperty("caps")] // humidities  per hour(24)
        public List<double> Caps { get; } = new List<double>();

        [JsonProperty("caps_base64")]
        public object CapsBase64 { get; set; }
    }


}
