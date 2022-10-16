using System.Diagnostics;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Utils
{
    /// <summary>
    /// Wrapper around System.Diagnostics.Process 
    /// </summary>
    public class ProcessStarter : IProcessStarter
    {
        public Process Start(string filename)
        {
            var info = new ProcessStartInfo(filename);
            return Process.Start(info);
        }

        public Process Start(ProcessStartInfo info)
        {
            return Process.Start(info);
        }
    }
}
