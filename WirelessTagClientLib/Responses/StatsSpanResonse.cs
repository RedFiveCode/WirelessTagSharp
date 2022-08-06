using Newtonsoft.Json;
using System.Collections.Generic;

namespace WirelessTagClientLib.RawResponses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class StatsSpanData
    {
        //[JsonProperty("__type")]
        //public string Type { get; set; }

        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("to")]
        public long To { get; set; }

        //[JsonProperty("tempBL")]
        //public object TempBL { get; set; }

        //[JsonProperty("capBL")]
        //public object CapBL { get; set; }

        //[JsonProperty("luxBL")]
        //public object LuxBL { get; set; }

        [JsonProperty("temp_unit")]
        public int TempUnit { get; set; }

        [JsonProperty("tzo")]
        public int Tzo { get; set; }

        [JsonProperty("ids")]
        public List<int> Ids { get; } = new List<int>();

        [JsonProperty("names")]
        public List<string> Names { get; } = new List<string>();

        [JsonProperty("discons")]
        public List<object> Discons { get; } = new List<object>();
    }

    public class MultiTagStatsSpanResponse
    {
        [JsonProperty("d")]
        public StatsSpanData Data { get; set; }
    }




}
