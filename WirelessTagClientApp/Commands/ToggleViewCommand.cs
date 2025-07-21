using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;

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
            foreach (var tag in viewModel.Tags)
            {
                tag.Mode = (_direction == Direction.Next ? EnumHelper.NextEnum<TagViewModel.ViewMode>(tag.Mode)
                                                        : EnumHelper.PreviousEnum<TagViewModel.ViewMode>(tag.Mode));
            }
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}
