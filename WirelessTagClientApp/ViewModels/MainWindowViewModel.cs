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
        private readonly IWirelessTagAsyncClient _client;
        private readonly Options _options;

        private DateTime _lastUpdated;
        private bool _isBusy;
        private bool _isError;
        private string _errorMessage;

        private readonly RefreshViewCommand _refreshCommand;
        private readonly CloseCommand _closeCommand;
        private readonly AboutCommand _aboutCommand;
        private readonly ChangeViewCommand _summaryViewCommand;
        private readonly ChangeViewCommand _minMaxViewCommand;
        private readonly DelegatedCommand _copyCommand;
        private readonly DelegatedCommand _toggleUnitsCommand;
        private readonly DelegatedCommand _copyRawDataCommand;

        private ViewMode _mode;
        private ViewModelBase _activeViewModel;
        private readonly Dictionary<ViewMode, ViewModelBase> _viewModelMap;

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
            this._client = client;
            this._options = options;
            _isBusy = false;
            _isError = false;
            _errorMessage = String.Empty;

            _viewModelMap = new Dictionary<ViewMode, ViewModelBase>();
            var summaryViewModel = new AllTagsViewModel(this);
            var minMaxViewModel = new MinMaxViewModel(this);

            _viewModelMap[ViewMode.SummaryView] = summaryViewModel;
            _viewModelMap[ViewMode.MinMaxView] = minMaxViewModel;

            Mode = ViewMode.SummaryView;

            _refreshCommand = new RefreshViewCommand();
            _closeCommand = new CloseCommand();
            _aboutCommand = new AboutCommand();
            _summaryViewCommand = new ChangeViewCommand(ViewMode.SummaryView);
            _minMaxViewCommand = new ChangeViewCommand(ViewMode.MinMaxView);
            _copyCommand = new DelegatedCommand();
            _toggleUnitsCommand = new DelegatedCommand();
            _copyRawDataCommand = new DelegatedCommand();

            // associate commands for main view that are delegated to the active child view(s)
            _copyCommand.Register(ViewMode.SummaryView, summaryViewModel.CopyCommand);
            _copyCommand.Register(ViewMode.MinMaxView, minMaxViewModel.CopyCommand);

            _toggleUnitsCommand.Register(ViewMode.MinMaxView, minMaxViewModel.ToggleTemperatureUnitsCommand);

            _copyRawDataCommand.Register(ViewMode.MinMaxView, minMaxViewModel.CopyRawDataCommand);
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
            get { return _lastUpdated; }
            set
            {
                _lastUpdated = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the busy flag
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get/set the error flag
        /// </summary>
        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
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
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;

                IsError = !String.IsNullOrEmpty(value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Get the command to refresh the active view
        /// </summary>
        public ICommand RefreshCommand
        {
            get { return _refreshCommand.Command; }
        }

        /// <summary>
        /// Get the command to close the app
        /// </summary>
        public ICommand CloseCommand
        {
            get { return _closeCommand.Command; }
        }

        /// <summary>
        /// Get the command to show About dialog
        /// </summary>
        public ICommand AboutCommand
        {
            get { return _aboutCommand.Command; }
        }

        /// <summary>
        /// Get the command to change the view
        /// </summary>
        public ICommand SummaryViewCommand
        {
            get { return _summaryViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to change the view
        /// </summary>
        public ICommand MinMaxViewCommand
        {
            get { return _minMaxViewCommand.Command; }
        }

        /// <summary>
        /// Get the command to copy the active view to the clipboard
        /// </summary>
        public ICommand CopyCommand
        {
            get { return _copyCommand.Command; }
        }

        /// <summary>
        /// Get the command to copy raw data from the min-max view to the clipboard
        /// </summary>
        public ICommand CopyRawDataCommand
        {
            get { return _copyRawDataCommand.Command; }
        }

        /// <summary>
        /// Get the command to toggle the temperature units
        /// </summary>
        public ICommand ToggleUnitsCommand
        {
            get { return _toggleUnitsCommand.Command; }
        }

        public ViewModelBase ActiveViewModel
        {
            get { return _activeViewModel; }

            set
            {
                _activeViewModel = value;
                NotifyPropertyChanged();
            }
        }

        public ViewMode Mode
        {
            get { return _mode; }

            set
            {
                _mode = value;
                ActiveViewModel = _viewModelMap[value];

                NotifyPropertyChanged();
            }
        }

        public IWirelessTagAsyncClient Client
        {
            get { return _client; }
        }

        public Options Options
        {
            get { return _options; }
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
                //IsBusy = true;

                // TODO - delegate to each view model to refresh using the respective command
                if (_mode == ViewMode.SummaryView)
                {
                    var viewModel = _viewModelMap[_mode] as AllTagsViewModel;

                    await viewModel.RefreshAsync()
                                 .ContinueWith(refreshTask =>
                                 {
                                     //LastUpdated = DateTime.Now;
                                     //IsBusy = false;

                                     if (refreshTask.IsFaulted)
                                     {
                                         SetError(refreshTask.Exception.ToString());
                                     }
                                 }, TaskScheduler.FromCurrentSynchronizationContext());

                }
                else if (_mode == ViewMode.MinMaxView)
                {
                    var viewModel = _viewModelMap[_mode] as MinMaxViewModel;

                    await viewModel.RefreshAsync()
                                   .ContinueWith(refreshTask =>
                                    {
                                        //LastUpdated = DateTime.Now;
                                        //IsBusy = false;
                                        if (refreshTask.IsFaulted)
                                        {
                                            SetError(refreshTask.Exception.ToString());
                                        }
                                    }, TaskScheduler.FromCurrentSynchronizationContext());

                    //await command.ExecuteAsync(viewModel)
                    //             .ContinueWith(refreshTask =>
                    //             {
                    //                 LastUpdated = DateTime.Now;
                    //                 IsBusy = false;

                    //                 if (refreshTask.IsFaulted)
                    //                 {
                    //                     SetError(refreshTask.Exception.ToString());
                    //                 }
                    //             }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Cannot refresh active view '{_mode}'");
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
