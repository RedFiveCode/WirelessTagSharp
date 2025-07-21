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
        private readonly MainWindowViewModel.ViewMode _mode;

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
            _mode = mode;
            Command = new RelayCommandT<MainWindowViewModel>(p => ChangeView(p));
        }

        private void ChangeView(MainWindowViewModel p)
        {
            p.Mode = _mode; // this will change the active view-model as well

            // update main view LastUpdated from active child's view-model
            if (p.Mode == MainWindowViewModel.ViewMode.SummaryView)
            {
                var childViewModel = p.ActiveViewModel as AllTagsViewModel;
                p.LastUpdated = childViewModel.LastUpdated;
            }
            else if (p.Mode == MainWindowViewModel.ViewMode.MinMaxView)
            {
                var childViewModel = p.ActiveViewModel as MinMaxViewModel;
                p.LastUpdated = childViewModel.LastUpdated;
            }
        }
    }
}
