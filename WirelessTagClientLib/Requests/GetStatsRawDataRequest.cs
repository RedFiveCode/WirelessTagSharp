using System;

namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to http://mytaglist.com/ethLogs.asmx?op=GetStatsRaw
    /// </summary>
    /// <remarks>
    /// For specified tag id, retrieves raw temperature/battery/humidity data for a given date range
    /// </remarks>
    internal class GetStatsRawDataRequest : JsonPostRequest
    {
        public GetStatsRawDataRequest() : base("/ethLogs.asmx/GetStatsRaw") { }

        /// <summary>
        /// Tag id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date from.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Date to.
        /// </summary>
        public DateTime To { get; set; }

        protected override object GetRequestBody()
        {
            string from = FormatDate(From);
            string to = FormatDate(To);

            return new { id = Id, fromDate = from, toDate = to };
        }

        private string FormatDate(DateTime dt)
        {
            return dt.ToString(@"M\/dd\/yyyy"); // "12/11/2015" for December 11, 2015
        }
    }
}
