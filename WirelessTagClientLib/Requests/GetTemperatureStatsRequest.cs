namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to qet average hourly temperature for the specified tag
    /// </summary>
    internal class GetTemperatureStatsRequest : JsonPostRequest
    {
        public GetTemperatureStatsRequest() : base("/ethLogs.asmx/GetTemperatureStats2 ") { }

        /// <summary>
        /// Tag id.
        /// </summary>
        public int Id { get; set; }

        protected override object GetRequestBody()
        {
            return new { id = Id };
        }
    }
}
