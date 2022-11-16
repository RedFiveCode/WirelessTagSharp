using AsyncAwaitBestPractices.MVVM;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Commands
{
    public class RefreshMinMaxTagsCommand
    {
        private readonly IWirelessTagAsyncClient client;
        private readonly Options options;

        public IAsyncCommand<MinMaxViewModel> Command { get; private set; }

        public RefreshMinMaxTagsCommand(IWirelessTagAsyncClient client, Options options)
        {
            this.client = client;
            this.options = options;
            Command = new AsyncCommand<MinMaxViewModel>(p => ExecuteAsync(p), p => CanExecute(p));
        }

        private bool CanExecute(object p)
        {
            return true;
        }

        public async Task ExecuteAsync(MinMaxViewModel viewModel)
        {
            List<Task> taskList = new List<Task>();

            try
            {
                // clear any previous results
                viewModel.Data.Clear();

                var loginTask = client.LoginAsync(options.Username, options.Password);

                Console.WriteLine($"LoginAsync task is {loginTask.Id}");

                await loginTask;

                if (!loginTask.Result)
                {
                    // cannot login
                    return;
                }

                var tagListTask = client.GetTagListAsync();

                Console.WriteLine($"GetTagListAsync task is {tagListTask.Id}");

                await tagListTask;

                var tagList = tagListTask.Result;

                if (tagList == null || tagList.Count == 0)
                {
                    // no tags
                    return;
                }

                // THIS WORKS!
                // Get UI update after each request completes,
                // but too many concurrent requests results in HttpStatusException / HttpStatusCode.InternalServerError
                foreach (var tag in tagList)
                {
                    //foreach (var interval in Enum.GetValues(typeof(TimeInterval)).Cast<TimeInterval>())
                    var interval = TimeInterval.Today;
                    {
                        await GetTemperatureRawDataWithContinuationTask(viewModel, tag, interval);
                    }
                }


                // THIS WORKS!
                // Get UI update after each request completes,
                // but too many concurrent requests results in HttpStatusException / HttpStatusCode.InternalServerError
                //foreach (var tag in tagListTask.Result)
                //{
                //    //var task = GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.Today);

                //    // Gotta be careful here an not spam the server with too many requests...
                //    // Will get HttpStatusException / HttpStatusCode.InternalServerError
                //    // {"Message":"You have downloaded full log less than 30 seconds ago. 
                //    // Please configure URL call back to receive real-time tag data instead of polling the API GetTemperatureRawData","StackTrace":"
                //    // at MyTagList.ethLogs.CheckLogDownloadRateLimit(...) 
                //    // at MyTagList.ethLogs.CheckTagType(...)
                //    // at MyTagList.ethLogs.GetTemperatureRawData(...)",
                //    // "ExceptionType":"MyTagList.ethLogs+TooManyRequestException"}

                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.Today));
                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.Yesterday));
                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.Last7Days));
                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.Last30Days));
                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.ThisYear));
                //    taskList.Add(GetTemperatureRawDataWithContinuationTask(tag, TimeInterval.All));
                //    // etc
                //}

                await Task.WhenAll(taskList);

                // THIS WORKS!
                //foreach (var tag in tagListTask.Result)
                //{
                //    // get all raw data for current tag
                //    // some tags may not have any data points in the time period of interest...
                //    // only query the client once to get all data, rather than repeated queries for ever increasing chunks of the same data
                //    // ideally should call GetTagSpanStatsAsync to get time range for each tag
                //    var from = new DateTime(2021, 1, 1);
                //    var to = DateTime.Now;

                //    var rawData = await client.GetTemperatureRawDataAsync(tag.SlaveId, from, to);

                //    // split raw data for current tag into time ranges (for example last 7 days)
                //    // and get min and max temperature for that time interval
                //    var rowList = new List<MinMaxMeasurementViewModel>();

                //    foreach (var interval in Enum.GetValues(typeof(TimeInterval)).Cast< TimeInterval>())
                //    {
                //        var row = CreateRowViewModel(rawData, tag, interval);

                //        if (row != null)
                //        {
                //            rowList.Add(row);
                //        }
                //    }


                //    if (rowList.Any())
                //    {
                //        foreach (var row in rowList)
                //        {
                //            viewModel.Data.Add(row);
                //        }
                //    }

                //    //taskList.Add(t);
                //}


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private Task GetTemperatureRawDataTask(int tagId, TimeInterval interval)
        {
            var timeRange = TimeIntervalHelper.GetTimeRange(DateTime.Now, interval);

            return client.GetTemperatureRawDataAsync(tagId, timeRange.Item1, timeRange.Item2);
        }

        private Task GetTemperatureRawDataWithContinuationTask(MinMaxViewModel viewModel, TagInfo tag, TimeInterval interval)
        {
            var timeRange = TimeIntervalHelper.GetTimeRange(DateTime.Now, interval);

            var task = client.GetTemperatureRawDataAsync(tag.SlaveId, timeRange.Item1, timeRange.Item2)
                .ContinueWith(rawDataTask =>
                {
                    // create row view-model object on worker thread;
                    // potentially has to filter a large number of raw data items
                    // so don't block the UI thread
                    if (rawDataTask.IsCompleted)
                    {
                        var rawData = rawDataTask.Result;

                        return ViewModelFactory.CreateRowViewModel(rawData, tag, interval);

                    }
                    return null; // error or no data for tag in the time interval
                })
                .ContinueWith(rowTask =>
                 {
                     // UI thread
                     if (rowTask.IsCompleted && rowTask.Result != null)
                     {
                         // UI thread
                         viewModel.Data.Add(rowTask.Result);
                         //Application.Current.Dispatcher.Invoke(() =>
                         //{
                         //    viewModel.Data.Add(rowTask.Result);
                         //});
                     }
                 }, TaskScheduler.FromCurrentSynchronizationContext());

            return task;
        }
    }
 }
