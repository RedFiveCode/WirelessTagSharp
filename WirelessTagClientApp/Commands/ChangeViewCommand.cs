using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to set view mode on main view view-model
    /// </summary>
    public class ChangeViewCommand
    {
        private MainWindowViewModel.ViewMode mode;

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mode"></param>
        public ChangeViewCommand(MainWindowViewModel.ViewMode mode)
        {
            this.mode = mode;
            Command = new RelayCommandT<MainWindowViewModel>(p => ChangeView(p));
        }

        private void ChangeView(MainWindowViewModel p)
        {
            p.Mode = mode; // this will change the active view-model as well
        }
    }
}
