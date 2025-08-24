namespace WirelessTagClientApp
{
    /// <summary>
    /// Application options
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Access token; see https://wirelesstag.net/eth/oauth2_apps.html"
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Cache folder; either relative path of rooted path
        /// </summary>
        public string CacheFolder { get; set; } = @"..\..\..\_cache";
    }
}
