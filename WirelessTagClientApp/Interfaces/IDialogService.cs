using System.Windows;

namespace WirelessTagClientApp.Interfaces
{
    /// <summary>
    /// Dialog box interface for unit testing
    /// </summary>
    public interface IDialogService
    {
        bool ShowDialog(Window ownerWindow);
    }
}

