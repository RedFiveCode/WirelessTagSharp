using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WirelessTagClientApp.Common;
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

                // IMPORTANT
                // We need to get historic temperature data for all the tags and time intervals.
                //
                // We can create tasks to send a request to get temperature data for a given tag and time interval,
                // and await each request/task to complete. However the server does not like too many requests too quickly...
                //
                // We hope to avoid too many concurrent requests as this can result in HttpStatusException / HttpStatusCode.InternalServerError
                // {"Message":"You have downloaded full log less than 30 seconds ago. 
                //   Please configure URL call back to receive real-time tag data instead of polling the API GetTemperatureRawData","StackTrace":"
                //   at MyTagList.ethLogs.CheckLogDownloadRateLimit(...) 
                //   at MyTagList.ethLogs.CheckTagType(...)
                //   at MyTagList.ethLogs.GetTemperatureRawData(...)",
                //   "ExceptionType":"MyTagList.ethLogs+TooManyRequestException"}
                //
                // Instead, we perform one query per tag to get all the temperature data over a long time period; will then split this into chunks locally.
                // Since each query potentially takes a long time, this avoids the risk of spamming the server with too many requests to quickly.
                //
                // Also, remember that some tags may not have any data points in the time period of interest.
                // Ideally should call GetTagSpanStatsAsync/GetMultiTagStatsSpan to get time range for each tag.
                //
                // Uses an arbitrary cutoff date as staring point for oldest data.
                var cutoffDate = new DateTime(2015, 1, 1);

                foreach (var tag in tagList)
                {
                    //if (viewModel.ContainsDataForTagAndInterval(tag.SlaveId, TimeInterval.Today))

                    if (viewModel.RawDataCache.ContainsDataForTag(tag.SlaveId, DateTime.Today))
                    {
                        cutoffDate = DateTime.Today;

                        Console.WriteLine($"Tag {tag.SlaveId} : Already has data for today; updating from {cutoffDate}");
                    }
                    else
                    {
                        Console.WriteLine($"Tag {tag.SlaveId} : Does not have data for today; updating from {cutoffDate}");
                    }

                    await GetTemperatureRawDataWithContinuationTask(viewModel, tag, cutoffDate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private Task GetTemperatureRawDataWithContinuationTask(MinMaxViewModel viewModel, TagInfo tag, DateTime from)
        {
            var to = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // end of today

            var stopwatch = Stopwatch.StartNew();
            var task = client.GetTemperatureRawDataAsync(tag.SlaveId, from, to)
                .ContinueWith(rawDataTask =>
                {
                    stopwatch.Stop();

                    viewModel.LastUpdated = DateTime.Now;

                    // create row view-model object on worker thread;
                    // potentially has to filter a large number of raw data items
                    // so don't block the UI thread
                    if (rawDataTask.IsCompleted && rawDataTask.Status == TaskStatus.RanToCompletion)
                    {
                        Console.WriteLine($"Tag {tag.SlaveId} : {rawDataTask.Result.Count} data points since {from}, duration {stopwatch.Elapsed}");

                        // Add data from this query to the cache
                        // This will maintain any existing data in the cache that overlaps with the response;
                        // that is raw data points for today already in the cache will be kept
                        // and further (more recent) raw data points for today not yet in the cache will be added to the cache
                        viewModel.RawDataCache.Update(tag.SlaveId, rawDataTask.Result);

                        // get all the data in the cache
                        var rawData = viewModel.RawDataCache.GetData(tag.SlaveId);

                        Console.WriteLine($"Tag {tag.SlaveId} : Cached {rawData.Count()} data points");

                        var rows = new List<MinMaxMeasurementViewModel>();

                        // split raw data for current tag into time ranges (for example last 7 days)
                        foreach (var interval in Enum.GetValues(typeof(TimeInterval)).Cast<TimeInterval>())
                        {
                            var rowViewModel = ViewModelFactory.CreateRowViewModel(rawData, tag, interval);

                            // some tags may not have any data points in the time period of interest
                            if (rowViewModel != null) // null means no data within the time interval
                            {
                                rows.Add(rowViewModel);
                            }
                        }

                        return rows;

                    }
                    return null; // error or no data for tag in the time interval
                })
                .ContinueWith(rowTask =>
                {
                    // UI thread
                    if (rowTask.IsCompleted && rowTask.Result != null)
                    {
                        // UI thread
                        foreach (var row in rowTask.Result)
                        {
                            viewModel.Data.Add(row);
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            return task;
        }
    }
 }
