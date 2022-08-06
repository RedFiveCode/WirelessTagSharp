namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Request to return if the client is sending authentication cookie.
    /// </summary>
    internal class IsSignedInRequest : JsonPostRequest
    {
        public IsSignedInRequest() : base("/ethAccount.asmx/IsSignedIn") { }

        protected override object GetRequestBody()
        {
            return new { /* request has no parameters */ };
        }
    }
}
