using CommandLine;

namespace WirelessTagClientCache
{
    [Verb("show", isDefault: false, HelpText = "Show cached files")]
    public class ShowOptions
    {
        [Option('f', "folder", Required = true, HelpText = "Folder path to store data.")]
        public string Folder { get; set; }

        [Option('t', "token", Required = true, HelpText = "Access token; see https://wirelesstag.net/eth/oauth2_apps.html")]
        public string AccessToken { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; }
    }
}
