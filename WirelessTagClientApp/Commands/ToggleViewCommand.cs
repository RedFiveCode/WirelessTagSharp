using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to change the view to the next view
    /// </summary>
    public class ToggleViewCommand
    {
        public enum Direction { Next = 0, Previous = -1 }

        private readonly Direction _direction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleViewCommand"/> class.
        /// </summary>
        public ToggleViewCommand() : this(Direction.Next) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleViewCommand"/> class.
        /// </summary>
        public ToggleViewCommand(Direction direction)
        {
            _direction = direction;
            Command = new RelayCommandT<AllTagsViewModel>(p => ToggleView(p));
        }

        private void ToggleView(AllTagsViewModel viewModel)
        {
            viewModel.Mode = (_direction == Direction.Next ? EnumHelper.NextEnum<AllTagsViewModel.ViewMode>(viewModel.Mode)
                                                           : EnumHelper.PreviousEnum<AllTagsViewModel.ViewMode>(viewModel.Mode));
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}
