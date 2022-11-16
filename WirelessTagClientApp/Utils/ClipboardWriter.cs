using System;
using System.Windows;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Utils
{
    /// <summary>
    /// Writes data to the clipboard.
    /// </summary>
    public class ClipboardWriter : IClipboardWriter
    {
        /// <summary>
        /// Write text to the clipboard.
        /// </summary>
        /// <param name="text"></param>
        public void WriteText(string text)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(text);
            }
            catch (Exception) { }
        }
    }
}
