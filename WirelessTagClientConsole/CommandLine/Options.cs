using CommandLine;

namespace WirelessTagClientConsole
{
    /// <summary>
    /// Stores command line options
    /// </summary>
    public class Options
    {
        [Option('u', "user", Required = true, HelpText = "User name")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "Password")]
        public string Password { get; set; }
    }
}
