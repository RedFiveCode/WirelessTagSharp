using System.Diagnostics;

namespace WirelessTagClientApp.Interfaces
{
    /// <summary>
    /// Wrapper around System.Diagnostics.Process 
    /// </summary>
    public interface IProcessStarter
    {
        Process Start(string filename);

        Process Start(ProcessStartInfo info);
    }
}
