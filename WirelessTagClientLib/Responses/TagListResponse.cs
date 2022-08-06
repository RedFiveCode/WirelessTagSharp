using Newtonsoft.Json;
using System.Collections.Generic;

namespace WirelessTagClientLib.RawResponses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class TagListData
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("dbid")]
        public int Dbid { get; set; }

        [JsonProperty("notificationJS")]
        public string NotificationJS { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("slaveId")]
        public int SlaveId { get; set; }

        [JsonProperty("tagType")]
        public int TagType { get; set; }

        [JsonProperty("discon")]
        public object Discon { get; set; }

        [JsonProperty("lastComm")]
        public long LastComm { get; set; }

        [JsonProperty("alive")]
        public bool Alive { get; set; }

        [JsonProperty("signaldBm")]
        public int SignaldBm { get; set; }

        [JsonProperty("batteryVolt")]
        public double BatteryVolt { get; set; }

        [JsonProperty("beeping")]
        public bool Beeping { get; set; }

        [JsonProperty("lit")]
        public bool Lit { get; set; }

        [JsonProperty("migrationPending")]
        public bool MigrationPending { get; set; }

        [JsonProperty("beepDurationDefault")]
        public int BeepDurationDefault { get; set; }

        [JsonProperty("eventState")]
        public int EventState { get; set; }

        [JsonProperty("tempEventState")]
        public int TempEventState { get; set; }

        [JsonProperty("OutOfRange")]
        public bool OutOfRange { get; set; }

        [JsonProperty("tempSpurTh")]
        public int TempSpurTh { get; set; }

        [JsonProperty("lux")]
        public int Lux { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("tempCalOffset")]
        public double TempCalOffset { get; set; }

        [JsonProperty("capCalOffset")]
        public int CapCalOffset { get; set; }

        [JsonProperty("image_md5")]
        public string ImageMd5 { get; set; }

        [JsonProperty("cap")]
        public double Cap { get; set; }

        [JsonProperty("capRaw")]
        public int CapRaw { get; set; }

        [JsonProperty("az2")]
        public int Az2 { get; set; }

        [JsonProperty("capEventState")]
        public int CapEventState { get; set; }

        [JsonProperty("lightEventState")]
        public int LightEventState { get; set; }

        [JsonProperty("shorted")]
        public bool Shorted { get; set; }

        [JsonProperty("zmod")]
        public object Zmod { get; set; }

        [JsonProperty("thermostat")]
        public object Thermostat { get; set; }

        [JsonProperty("playback")]
        public object Playback { get; set; }

        [JsonProperty("postBackInterval")]
        public int PostBackInterval { get; set; }

        [JsonProperty("rev")]
        public int Rev { get; set; }

        [JsonProperty("version1")]
        public int Version1 { get; set; }

        [JsonProperty("freqOffset")]
        public int FreqOffset { get; set; }

        [JsonProperty("freqCalApplied")]
        public int FreqCalApplied { get; set; }

        [JsonProperty("reviveEvery")]
        public int ReviveEvery { get; set; }

        [JsonProperty("oorGrace")]
        public int OorGrace { get; set; }

        [JsonProperty("tempBL")]
        public object TempBL { get; set; }

        [JsonProperty("capBL")]
        public object CapBL { get; set; }

        [JsonProperty("luxBL")]
        public object LuxBL { get; set; }

        [JsonProperty("LBTh")]
        public double LBTh { get; set; }

        [JsonProperty("enLBN")]
        public bool EnLBN { get; set; }

        [JsonProperty("txpwr")]
        public int Txpwr { get; set; }

        [JsonProperty("rssiMode")]
        public bool RssiMode { get; set; }

        [JsonProperty("ds18")]
        public bool Ds18 { get; set; }

        [JsonProperty("v2flag")]
        public int V2flag { get; set; }

        [JsonProperty("batteryRemaining")]
        public double BatteryRemaining { get; set; }

        [JsonProperty("usingSHT35")]
        public bool UsingSHT35 { get; set; }

        [JsonProperty("usingRCO")]
        public bool UsingRCO { get; set; }
    }

    public class TagListResponse
    {
        [JsonProperty("d")]
        public List<TagListData> Data { get; } = new List<TagListData>();
    }


}
