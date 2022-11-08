using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to refresh the main view
    /// </summary>
    public class RefreshViewCommand
    {
        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mode"></param>
        public RefreshViewCommand()
        {
            Command = new RelayCommandT<MainWindowViewModel>(p => Refresh(p), p => CanRefresh(p));
        }

        private bool CanRefresh(MainWindowViewModel p)
        {
            return true;
        }

        private void Refresh(MainWindowViewModel p)
        {
            p.Refresh();
        }
    }
}
