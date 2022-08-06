using System;

namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to get raw temperature data within a timespan
    /// </summary>
    internal class GetTemperatureRawDataRequest : JsonPostRequest
    {
        public GetTemperatureRawDataRequest() : base("/ethLogs.asmx/GetTemperatureRawData") { }

        /// <summary>
        /// Tag id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// From date
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// To date
        /// </summary>
        public DateTime To { get; set; }

        protected override object GetRequestBody()
        {
            return new { id = Id, fromDate = FormatDate(From), toDate = FormatDate(To) };
        }

        private string FormatDate(DateTime dt)
        {
            return dt.ToString(@"M\/dd\/yyyy"); // want M/DD/YYYY eg "6/26/2014"
        }
    }
}
