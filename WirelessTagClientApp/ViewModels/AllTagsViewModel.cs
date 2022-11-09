using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.ViewModels
{
    [DebuggerDisplay("Count={tagList.Count}")]
    public class AllTagsViewModel : ViewModelBase
    {
        private ToggleViewCommand toggleNextViewCommand;
        private ToggleViewCommand togglePreviousViewCommand;
        private ObservableCollection<TagViewModel> tagList;

        /// <summary>
        /// Ctor
        /// </summary>
        public AllTagsViewModel()
        {
            Tags = new ObservableCollection<TagViewModel>();

            toggleNextViewCommand = new ToggleViewCommand();
            togglePreviousViewCommand = new ToggleViewCommand(Commands.ToggleViewCommand.Direction.Previous);
        }

        /// <summary>
        /// Get/set the list of tags
        /// </summary>
        public ObservableCollection<TagViewModel> Tags
        {
            get { return tagList; }
            set
            {
                tagList = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get the command to toggle the view
        /// </summary>
        public ICommand ToggleNextViewCommand
        {
            get { return toggleNextViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the view
        /// </summary>
        public ICommand TogglePreviousViewCommand
        {
            get { return togglePreviousViewCommand.Command; }
        }

    }
}
