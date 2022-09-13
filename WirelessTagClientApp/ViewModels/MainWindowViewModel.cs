using System;
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

        private RefreshCommand refreshCommand;
        private CloseCommand closeCommand;
        // need new refresh command that will push a message on the bus to tell the active view to refresh itself???

        private AllTagsViewModel activeViewModel;

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

            refreshCommand = new RefreshCommand();
            closeCommand = new CloseCommand();
            activeViewModel = new AllTagsViewModel(this.options);
        }

        // Refresh the active view
        public void Refresh()
        {
            activeViewModel.Refresh();

            LastUpdated = DateTime.Now;
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
            get { return refreshCommand.Command; }
        }

        /// <summary>
        /// Get the command to close the app
        /// </summary>
        public ICommand CloseCommand
        {
            get { return closeCommand.Command; }
        }

        public AllTagsViewModel ActiveViewModel
        {
            get { return activeViewModel; }
        }

    }
}
