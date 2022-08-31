using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to refresh the view-model.
    /// </summary>
    public class RefreshCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshCommand"/> class.
        /// </summary>
        public RefreshCommand()
        {
            Command = new RelayCommandT<AllTagsViewModel>(p => Refresh(p), p => CanRefresh(p));
        }

        private bool CanRefresh(AllTagsViewModel viewModel)
        {
            return true;
        }

        private void Refresh(AllTagsViewModel viewModel)
        {
            viewModel.Refresh();
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}
