using System.Windows;
using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.Views;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to display About dialog box
    /// </summary>
    public class AboutCommand
    {
        private IDialogService dialogService;

        public AboutCommand() : this(new AboutDialogService())
        {}

        public AboutCommand(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            Command = new RelayCommandT<Window>(p => ShowAboutDialog(p));
        }

        private void ShowAboutDialog(Window ownerWindow)
        {
            Window owner = null;
            if (ownerWindow != null)
            {
                owner = ownerWindow;
            }
            else if (Application.Current != null)
            {
                owner = Application.Current.MainWindow;
            }

            dialogService.ShowDialog(owner);
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}

