using WirelessTagClientLib.DTO;

namespace WirelessTagClientConsole
{
    internal class TagTemperatureSpan
    {
        public TagInfo Tag { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
