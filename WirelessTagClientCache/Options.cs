using CommandLine;
using System;

namespace WirelessTagClientCache
{
    [Verb("write", isDefault: true, HelpText = "Write historic measurements to cache")]
    public class Options
    {
        [Option('f', "folder", Required = true, HelpText = "Folder path to store data.")]
        public string Folder { get; set; }

        [Option('i', "id", Required = true, HelpText = "Tag Id (integer).")]
        public int TagId { get; set; }

        [Option("from", Required = true, HelpText = "Start date (yyyy-MM-dd).")]
        public DateTime From { get; set; }

        [Option("to", Required = true, HelpText = "End date (yyyy-MM-dd).")]
        public DateTime To { get; set; }

        [Option('t', "token", Required = true, HelpText = "Access token; see https://wirelesstag.net/eth/oauth2_apps.html")]
        public string AccessToken { get; set; }

        [Option('c', "chunkSize", Required = false, HelpText = "Chunk size in days.")]
        public int ChunkSize{ get; set; } = -1;

        [Option('w', "waitInterval", Required = false, HelpText = "Wait interval in seconds between calls to prevent spamming the server.")]
        public int WaitInterval { get; set; } = -1;

        [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
        public bool Verbose { get; set; }
    }
}
