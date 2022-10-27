﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;
using WirelessTagClientLib;

namespace WirelessTagClientApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IWirelessTagAsyncClient client;
        private readonly Options options;

        private DateTime lastUpdated;
        private bool isBusy;
        private bool isError;
        private string errorMessage;

        private RefreshAllTagsCommand refreshAllTagsCommand;
        private CloseCommand closeCommand;
        private AboutCommand aboutCommand;
        private ChangeViewCommand summaryViewCommand;
        private ChangeViewCommand minMaxViewCommand;

        private ViewMode mode;
        private ViewModelBase activeViewModel;
        private readonly Dictionary<ViewMode, ViewModelBase> viewModelMap;

        public enum ViewMode { SummaryView = 0, MinMaxView }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        public MainWindowViewModel() : this(null, null)
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="options"></param>
        public MainWindowViewModel(Options options) : this(new WirelessTagAsyncClient(), options)
        { }

        /// <summary>
        /// Ctor for unit testing and common overload.
        /// </summary>
        /// <param name="client"></param>
        public MainWindowViewModel(IWirelessTagAsyncClient client, Options options)
        {
            this.client = client;
            this.options = options;
            isBusy = false;
            isError = false;
            errorMessage = String.Empty;

            refreshAllTagsCommand = new RefreshAllTagsCommand(client, options);
            closeCommand = new CloseCommand();
            aboutCommand = new AboutCommand();
            summaryViewCommand = new ChangeViewCommand(ViewMode.SummaryView);
            minMaxViewCommand = new ChangeViewCommand(ViewMode.MinMaxView);

            viewModelMap = new Dictionary<ViewMode, ViewModelBase>();
            viewModelMap[ViewMode.SummaryView] = new AllTagsViewModel(this.options);
            viewModelMap[ViewMode.MinMaxView] = new MinMaxViewModel(this.options);

            Mode = ViewMode.SummaryView;
        }


        /// <summary>
        /// Log-in and refresh the active view
        /// </summary>
        public void LoginAndRefresh()
        {
            // TODO
            // create watcher
            // add async refresh method to AllTagsViewModel
            // should we have one long lived instance of the watcher,
            // or create new watchers for each task/refresh call?

            // update LastUpdated when the watcher has completed
            // (subscribe to its property changed events or extend watcher to fired its own events?)
            //
            // maybe separate the watcher ctor from running the task as well, so can rerun the same task.

            //watcher = new NotifyTaskCompletion<AllTagsViewModel>(activeViewModel.RefreshAsync);

            // need to move client access to own service class away from AllTagsViewModel ???

            var task = client.LoginAsync(options.Username, options.Password);
            task.ContinueWith(responseTask =>
            {
                if (!responseTask.Result || responseTask.Status != TaskStatus.RanToCompletion)
                {
                    // UI thread ???
                    IsBusy = false;
                    SetError(Properties.Resources.Error_Login);
                }

                refreshAllTagsCommand.Command.Execute(this);

                LastUpdated = DateTime.Now;
            });
        }

        /// <summary>
        /// Get/set the time last updated from the server
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
        /// Get/set the busy flag
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the error flag
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the error message
        /// </summary>
        /// <remarks>
        /// Sets/resets the <see cref="IsError"/> property
        /// </remarks>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;

                IsError = !String.IsNullOrEmpty(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get the command to refresh the tag data
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return refreshAllTagsCommand.Command; }
        }

        /// <summary>
        /// Get the command to close the app
        /// </summary>
        public ICommand CloseCommand
        {
            get { return closeCommand.Command; }
        }

        /// <summary>
        /// Get the command to show About dialog
        /// </summary>
        public ICommand AboutCommand
        {
            get { return aboutCommand.Command; }
        }

        /// <summary>
        /// Get the command to change the view
        /// </summary>
        public ICommand SummaryViewCommand
        {
            get { return summaryViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to change the view
        /// </summary>
        public ICommand MinMaxViewCommand
        {
            get { return minMaxViewCommand.Command; }
        }

        public ViewModelBase ActiveViewModel
        {
            get { return activeViewModel; }

            set
            {
                activeViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public ViewMode Mode
        {
            get { return mode; }

            set
            {
                mode = value;
                ActiveViewModel = viewModelMap[value];

                NotifyPropertyChanged();
            }
        }

        public void SetError(string message)
        {
            IsBusy = false;
            IsError = true;
            ErrorMessage = message;
        }
    }
}
