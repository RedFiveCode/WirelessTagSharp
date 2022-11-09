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
        private readonly IWirelessTagAsyncClient client;
        private readonly Options options;

        public IAsyncCommand<AllTagsViewModel> Command { get; private set; }

        public RefreshAllTagsCommand(IWirelessTagAsyncClient client, Options options)
        {
            this.client = client;
            this.options = options;
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

                await client.GetTagListAsync()
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
        }

        private void OnGetTagListResponse(Task<List<TagInfo>> responseTask, AllTagsViewModel viewModel)
        {
            // UI thread ???
            if (responseTask.Status == TaskStatus.RanToCompletion)
            {
                // keep existing view mode of tags
                var originalViewMode = (viewModel.Tags.Any() ? viewModel.Tags.First().Mode : default(TagViewModel.ViewMode));

                viewModel.Tags = ViewModelFactory.CreateTagViewModelList(responseTask.Result, originalViewMode);
            }
            else
            {
                if (responseTask.Exception != null && responseTask.Exception.InnerException != null)
                {
                    throw new AggregateException(responseTask.Exception.InnerException);
                }
            }
        }
    }
 }
