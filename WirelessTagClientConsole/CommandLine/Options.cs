using CommandLine;

namespace WirelessTagClientConsole
{
    /// <summary>
    /// Stores command line options
    /// </summary>
    public class Options
    {
        [Option('u', "user", Required = false, HelpText = "User name")]
        public string Username { get; set; }

        [Option('p', "password", Required = false, HelpText = "Password")]
        public string Password { get; set; }

        [Option('t', "token", Required = true, HelpText = "Access token; see https://wirelesstag.net/eth/oauth2_apps.html")]
        public string AccessToken { get; set; }
    }
}
