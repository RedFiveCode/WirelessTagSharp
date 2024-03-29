﻿using System;
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
        private CopyAllTagsCommand copyCommand;
        private ObservableCollection<TagViewModel> tagList;
        private DateTime lastUpdated;

        /// <summary>
        /// Ctor
        /// </summary>
        public AllTagsViewModel()
        {
            Tags = new ObservableCollection<TagViewModel>();
            lastUpdated = DateTime.MinValue;

            toggleNextViewCommand = new ToggleViewCommand();
            togglePreviousViewCommand = new ToggleViewCommand(ToggleViewCommand.Direction.Previous);
            copyCommand = new CopyAllTagsCommand();
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
        /// Get/set the time last updated
        /// </summary>
        public DateTime LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
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

        /// <summary>
        /// Get the command to copy the data for all tags to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return copyCommand.Command; }
        }
    }
}
