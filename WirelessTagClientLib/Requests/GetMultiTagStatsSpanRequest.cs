using System.Collections.Generic;

namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to get the date of the earliest and latest available data for each tag
    /// </summary>
    internal class GetMultiTagStatsSpanRequest : JsonPostRequest
    {
        public GetMultiTagStatsSpanRequest() : base("/ethLogs.asmx/GetMultiTagStatsSpan2") { }

        /// <summary>
        /// Tag id.
        /// </summary>
        public List<int> Ids { get; set; }

        protected override object GetRequestBody()
        {
            return new { ids = Ids, type = "", sinceLastCalibration = "false" };
        }
    }
}
