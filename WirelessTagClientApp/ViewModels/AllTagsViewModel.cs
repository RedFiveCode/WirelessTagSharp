using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.ViewModels
{
    [DebuggerDisplay("Count={tagList.Count}")]
    public class AllTagsViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _parentViewModel;

        private readonly ToggleViewCommand _toggleNextViewCommand;
        private readonly ToggleViewCommand _togglePreviousViewCommand;
        private readonly CopyAllTagsCommand _copyCommand;
        private readonly RefreshAllTagsCommand _refreshCommand;
        private ObservableCollection<TagViewModel> _tagList;
        private DateTime _lastUpdated;

        /// <summary>
        /// Ctor
        /// </summary>
        public AllTagsViewModel(MainWindowViewModel parentViewModel = null)
        {
            _parentViewModel = parentViewModel;

            Tags = new ObservableCollection<TagViewModel>();
            _lastUpdated = DateTime.MinValue;

            _toggleNextViewCommand = new ToggleViewCommand();
            _togglePreviousViewCommand = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            _copyCommand = new CopyAllTagsCommand();
            _refreshCommand = new RefreshAllTagsCommand(parentViewModel?.Client, parentViewModel?.Options);
        }

        public async Task RefreshAsync()
        {
            await _refreshCommand.ExecuteAsync(this);
        }

        /// <summary>
        /// Get/set the list of tags
        /// </summary>
        public ObservableCollection<TagViewModel> Tags
        {
            get { return _tagList; }
            set
            {
                _tagList = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the time last updated
        /// </summary>
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                _lastUpdated = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get the command to toggle the view
        /// </summary>
        public ICommand ToggleNextViewCommand
        {
            get { return _toggleNextViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the view
        /// </summary>
        public ICommand TogglePreviousViewCommand
        {
            get { return _togglePreviousViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to copy the _data for all tags to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return _copyCommand.Command; }
        }

        /// <summary>
        /// Get the command to refresh the view
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return _refreshCommand.Command; }
        }

        /// <summary>
        /// Get the parent view-model (for command routing)
        /// </summary>
        public MainWindowViewModel ParentViewModel
        {
            get { return _parentViewModel; }
        }
    }
}
