using AsyncAwaitBestPractices.MVVM;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Commands
{
    public class RefreshAllTagsCommand
    {
        private readonly IWirelessTagAsyncClient _client;
        private readonly Options _options;

        public IAsyncCommand<AllTagsViewModel> Command { get; private set; }

        public RefreshAllTagsCommand(IWirelessTagAsyncClient client, Options options)
        {
            _client = client;
            _options = options;
            Command = new AsyncCommand<AllTagsViewModel>(p => ExecuteAsync(p), p => CanExecute(p));
        }

        private bool CanExecute(object p)
        {
            return true;
        }

        public async Task ExecuteAsync(AllTagsViewModel viewModel)
        {
            try
            {
                // uncomment to simulate a very long delay in getting response
                //await Task.Delay(5000);

                //var loginTask = _client.LoginAsync(_options.Username, _options.Password);

                //await loginTask;

                // set busy overlay
                viewModel.ParentViewModel.IsBusy = true;

                await _client.GetTagListAsync()
                        .ContinueWith(tt =>
                        {
                            // UI thread
                            OnGetTagListResponse(tt, viewModel);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // reset busy overlay
                viewModel.ParentViewModel.LastUpdated = DateTime.Now;
                viewModel.ParentViewModel.IsBusy = false;
            }
        }

        private void OnGetTagListResponse(Task<List<TagInfo>> responseTask, AllTagsViewModel viewModel)
        {
            // UI thread ???
            if (responseTask.Status == TaskStatus.RanToCompletion)
            {
                viewModel.Tags = ViewModelFactory.CreateTagViewModelList(responseTask.Result);
            }
            else
            {
                if (responseTask.Exception != null && responseTask.Exception.InnerException != null)
                {
                    throw new AggregateException(responseTask.Exception.InnerException);
                }
            }

            viewModel.LastUpdated = DateTime.Now;
        }
    }
 }
