using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
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
        public IAsyncCommand<MainWindowViewModel> Command { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mode"></param>
        public RefreshViewCommand()
        {
            Command = new AsyncCommand<MainWindowViewModel>(p => RefreshAsync(p), p => CanRefresh(p));
        }

        private bool CanRefresh(object p)
        {
            return true;
        }

        private async Task RefreshAsync(MainWindowViewModel p)
        {
            await p.Refresh();
        }
    }
}
