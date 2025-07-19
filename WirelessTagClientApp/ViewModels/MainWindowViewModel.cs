using System;
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

        private RefreshViewCommand refreshCommand;
        private CloseCommand closeCommand;
        private AboutCommand aboutCommand;
        private ChangeViewCommand summaryViewCommand;
        private ChangeViewCommand minMaxViewCommand;
        private DelegatedCommand copyCommand;
        private DelegatedCommand toggleUnitsCommand;
        private DelegatedCommand copyRawDataCommand;

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
        public MainWindowViewModel(Options options) : this(new WirelessTagAsyncClient(options.AccessToken), options)
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

            viewModelMap = new Dictionary<ViewMode, ViewModelBase>();
            var summaryViewModel = new AllTagsViewModel();
            var minMaxViewModel = new MinMaxViewModel();

            viewModelMap[ViewMode.SummaryView] = summaryViewModel;
            viewModelMap[ViewMode.MinMaxView] = minMaxViewModel;

            Mode = ViewMode.SummaryView;

            refreshCommand = new RefreshViewCommand();
            closeCommand = new CloseCommand();
            aboutCommand = new AboutCommand();
            summaryViewCommand = new ChangeViewCommand(ViewMode.SummaryView);
            minMaxViewCommand = new ChangeViewCommand(ViewMode.MinMaxView);
            copyCommand = new DelegatedCommand();
            toggleUnitsCommand = new DelegatedCommand();
            copyRawDataCommand = new DelegatedCommand();

            // associate commands for main view that are delegated to the active child view(s)
            copyCommand.Register(ViewMode.SummaryView, summaryViewModel.CopyCommand);
            copyCommand.Register(ViewMode.MinMaxView, minMaxViewModel.CopyCommand);

            toggleUnitsCommand.Register(ViewMode.MinMaxView, minMaxViewModel.ToggleTemperatureUnitsCommand);

            copyRawDataCommand.Register(ViewMode.MinMaxView, minMaxViewModel.CopyRawDataCommand);
        }


        /// <summary>
        /// Log-in and refresh the active view
        /// </summary>
        public async void LoginAndRefresh()
        {
            await Refresh();
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
        /// Get the command to refresh the active view
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return refreshCommand.Command; }
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

        /// <summary>
        /// Get the command to copy the active view to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return copyCommand.Command; }
        }

        /// <summary>
        /// Get the command to copy raw data from the min-max view to the clipboard
        /// </summary>
        public ICommand CopyRawDataCommand
        {
            get { return copyRawDataCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the temperature units
        /// </summary>
        public ICommand ToggleUnitsCommand
        {
            get { return toggleUnitsCommand.Command; }
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

        public async Task Refresh()
        {
            try
            {
                IsBusy = true;

                if (mode == ViewMode.SummaryView)
                {
                    var command = new RefreshAllTagsCommand(client, options);
                    var viewModel = viewModelMap[mode] as AllTagsViewModel;

                    await command.ExecuteAsync(viewModel)
                                 .ContinueWith(refreshTask =>
                                 {
                                     LastUpdated = DateTime.Now;
                                     IsBusy = false;

                                     if (refreshTask.IsFaulted)
                                     {
                                         SetError(refreshTask.Exception.ToString());
                                     }
                                 }, TaskScheduler.FromCurrentSynchronizationContext());

                }
                else if (mode == ViewMode.MinMaxView)
                {
                    var command = new RefreshMinMaxTagsCommand(client, options);
                    var viewModel = viewModelMap[mode] as MinMaxViewModel;

                    await command.ExecuteAsync(viewModel)
                                 .ContinueWith(refreshTask =>
                                 {
                                     LastUpdated = DateTime.Now;
                                     IsBusy = false;

                                     if (refreshTask.IsFaulted)
                                     {
                                         SetError(refreshTask.Exception.ToString());
                                     }
                                 }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Cannot refresh active view '{mode}'");
                }
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }
            finally
            {
                //LastUpdated = DateTime.Now;
                //IsBusy = false;
            }
        }
    }
}
