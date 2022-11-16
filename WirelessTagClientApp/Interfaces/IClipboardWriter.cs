namespace WirelessTagClientApp.Interfaces
{
    /// <summary>
    /// Interface to write to the clipboard.
    /// </summary>
    public interface IClipboardWriter
    {
        /// <summary>
        /// Write text to the clipboard.
        /// </summary>
        /// <param name="text"></param>
        void WriteText(string text);
    }
}
