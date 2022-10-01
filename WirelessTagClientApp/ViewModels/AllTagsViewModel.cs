using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;
using WirelessTagClientLib;

namespace WirelessTagClientApp.ViewModels
{
    [DebuggerDisplay("Count={tagList.Count}")]
    public class AllTagsViewModel : ViewModelBase
    {
        private readonly IWirelessTagAsyncClient client;
        private readonly Options options;

        private ToggleViewCommand toggleNextViewCommand;
        private ToggleViewCommand togglePreviousViewCommand;
        private CloseCommand closeCommand;
        private ObservableCollection<TagViewModel> tagList;
        private DateTime lastUpdated;
        private bool isBusy;
        private bool isError;
        private string errorMessage;

        public AllTagsViewModel(Options options = null)
        {
            this.options = options;

            Tags = new ObservableCollection<TagViewModel>();
            LastUpdated = DateTime.MinValue;

            toggleNextViewCommand = new ToggleViewCommand();
            togglePreviousViewCommand = new ToggleViewCommand(Commands.ToggleViewCommand.Direction.Previous);
            closeCommand = new CloseCommand();

            client = new WirelessTagAsyncClient();

            isBusy = false;
            isError = false;
            errorMessage = String.Empty;
        }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        /// <param name="client"></param>
        public AllTagsViewModel(IWirelessTagAsyncClient client)
        {
            this.client = client;
            options = new Options();
        }


        /// <summary>
        /// Refresh the list of tags
        /// </summary>
        public void Refresh()
        {
            IsBusy = true;
            ClearError();


            // TODO - further decouple by publishing event when async call completes; event handler deals with task / payload
            client.LoginAsync(options.Username, options.Password)
                  .ContinueWith(t =>
            {
                if (!t.Result || t.Status != TaskStatus.RanToCompletion)
                {
                    SetError(Properties.Resources.Error_Login);
                    IsBusy = false;

                    return;
                }

                // TODO UI thread???
                client.GetTagListAsync().ContinueWith(tt =>
                {
                    IsBusy = false;
                    LastUpdated = DateTime.Now; // local time

                    if (tt.Status == TaskStatus.RanToCompletion)
                    {
                        Tags = ViewModelFactory.CreateTagViewModelList(tt.Result);
                    }
                    else
                    {
                        var ex = tt.Exception.InnerException;

                        SetError(String.Format(Properties.Resources.Error_Exception, ex.Message, ex.StackTrace));
                    }
                });
            });
 
            // TODO
            //Tags.Add(new TagViewModel() { Id = 1, Name = "My dummy tag", Description = "Some descriptive text", Temperature = 20, Uuid = Guid.Empty });
            //Tags.Add(new TagViewModel() { Id = 2, Name = "Second tag", Description = "Some descriptive text", Temperature = 21, Uuid = Guid.Empty });
            //Tags.Add(new TagViewModel() { Id = 3, Name = "Another tag with a very long name", Description = "Some descriptive text", Temperature = 22, Uuid = Guid.Empty });
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
        /// Get/set the error flag
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
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
        /// Get the command to close the app
        /// </summary>
        public ICommand CloseCommand
        {
            get { return closeCommand.Command; }
        }

        private void SetError(string message)
        {
            IsError = true;
            ErrorMessage = message;
        }

        private void ClearError()
        {
            IsError = false;
            ErrorMessage = String.Empty;
        }
    }
}
