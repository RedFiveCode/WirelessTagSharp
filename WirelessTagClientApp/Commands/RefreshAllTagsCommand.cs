﻿using AsyncAwaitBestPractices.MVVM;
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

        public IAsyncCommand<MainWindowViewModel> Command { get; private set; }

        public RefreshAllTagsCommand(IWirelessTagAsyncClient client, Options options)
        {
            this.client = client;
            this.options = options;
            Command = new AsyncCommand<MainWindowViewModel>(p => ExecuteAsync(p), p => CanExecute(p));
        }

        private bool CanExecute(object p)
        {
            return true;
            //var vm = p as MainWindowViewModel;
            //return vm != null && !vm.IsBusy;
        }

        public async Task ExecuteAsync(MainWindowViewModel viewModel)
        {
            try
            {
                viewModel.IsBusy = true;

                // uncomment to simulate a very long delay in getting response
                //await Task.Delay(5000);

                await client.GetTagListAsync()
                        .ContinueWith(tt =>
                        {
                            OnGetTagListResponse(tt, viewModel);
                        });
            }
            catch (Exception ex)
            {
                viewModel.SetError(ex.Message);
            }
            finally
            {
                viewModel.IsBusy = false;
            }
        }

        private void OnGetTagListResponse(Task<List<TagInfo>> responseTask, MainWindowViewModel viewModel)
        {
            // UI thread ???
            viewModel.IsBusy = false;
            viewModel.LastUpdated = DateTime.Now; // local time

            if (responseTask.Status == TaskStatus.RanToCompletion)
            {
                if (viewModel.Mode == MainWindowViewModel.ViewMode.SummaryView && viewModel.ActiveViewModel is AllTagsViewModel)
                {
                    var vm = viewModel.ActiveViewModel as AllTagsViewModel;

                    // keep existing view mode of tags
                    var originalViewMode = (vm.Tags.Any() ? vm.Tags.First().Mode : default(TagViewModel.ViewMode));

                    vm.Tags = ViewModelFactory.CreateTagViewModelList(responseTask.Result, originalViewMode);
                }
                else
                {
                    viewModel.SetError($"GetTagListResponse received when active view is not {MainWindowViewModel.ViewMode.SummaryView}");
                }
            }
            else
            {
                var ex = responseTask.Exception.InnerException;

                viewModel.SetError(String.Format(Properties.Resources.Error_Exception, ex.Message, ex.StackTrace));
            }
        }
    }
 }
