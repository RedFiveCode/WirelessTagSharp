namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to get a list of tags for the currently logged in account
    /// </summary>
    internal class GetTagListRequest : JsonPostRequest
    {
        public GetTagListRequest() : base("/ethClient.asmx/GetTagList") { }

        protected override object GetRequestBody()
        {
            return new { /* request has no parameters */ };
        }
    }
}
