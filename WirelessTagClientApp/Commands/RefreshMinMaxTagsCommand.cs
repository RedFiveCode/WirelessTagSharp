using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            //var cutoffDate = new DateTime(2020, 1, 1); // HACK using more recent date to avoid getting too much data and overflowing maximum json response size, also faster
            var cutoffDate = new DateTime(2015, 1, 1);

            try
            {
                // set busy overlay
                viewModel.ParentViewModel.IsBusy = true;

                // clear any previous results
                //viewModel.Data.Clear();

                var tagList = await client.GetTagListAsync();

                if (tagList == null || !tagList.Any())
                {
                    // no tags
                    return;
                }

                //tagList = tagList.Where(t => t.Name.Contains("#2")).ToList();

                // always get latest data for today for all tags
                await GetTodayDataForTags(viewModel, tagList);

                // get recent historical data (1st January this year through to end of yesterday)
                await GetRecentDataForTags(viewModel, tagList);

                // TODO - get older historical data (before 1st January this year)
                // TODO - only need to do this once, so guard/check
                // TODO - need to balance spaming server/generating too much data on the server
                // await GetHistoricDataForTags(viewModel, tagList, cutoffDate);

                //var porchTag = tagList.FirstOrDefault(t => t.Name.Contains("Study"));
                //if (porchTag != null)
                //{
                //    var porchData = viewModel.RawDataCache.GetData(porchTag.SlaveId);

                //    var allData = CreatCSVData(porchData);

                //    var currentYearData = CreatCSVData(porchData.Where(d => d.Time.Year == 2025));

                //    File.WriteAllText(@"c:\temp\Study-all.csv", allData);
                //    File.WriteAllText(@"c:\temp\Study 2025.csv", currentYearData);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                // reset busy overlay
                viewModel.ParentViewModel.LastUpdated = DateTime.Now;
                viewModel.ParentViewModel.IsBusy = false;
            }
        }

        public async Task ExecuteAsyncOld(MinMaxViewModel viewModel)
        {
            var today = DateTime.Now.Date;
            var cutoffDates = new List<DateTime> { today, today.AddDays(-1), today.AddDays(-2), today.AddDays(-7), new DateTime(2015, 1, 1) };

            try
            {
                // clear any previous results
                viewModel.Data.Clear();

                //var loginTask = client.LoginAsync(options.Username, options.Password);

               // Console.WriteLine($"LoginAsync task is {loginTask.Id}");

                //await loginTask;

                //if (!loginTask.Result)
                //{
                //    // cannot login
                //    return;
                //}

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
                //var cutoffDate = new DateTime(2015, 1, 1);

                foreach (var tag in tagList)
                {

                    var cutoffDate = new DateTime(2015, 1, 1);

                    if (viewModel.RawDataCache.ContainsDataForTag(tag.SlaveId, DateTime.Today))
                    {
                        cutoffDate = DateTime.Today;

                        Console.WriteLine($"Tag {tag.SlaveId} : Already has data for today; updating from {cutoffDate}");
                    }
                    else
                    {
                        Console.WriteLine($"Tag {tag.SlaveId} : Does not have data for today; updating from {cutoffDate}");
                    }

                    await GetTemperatureRawDataWithContinuationTask(viewModel, tag, today);
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


        private async Task GetHistoricDataForTags(MinMaxViewModel viewModel, List<TagInfo> tags, DateTime baseline)
        {
            // assume that the day, week, month of year does not roll over during the time this method is running
            // assume that the cache is empty or does not contain any data for the tags in the time period of interest

            // need to get data from the beginning of time (the baseline)
            // until the end of the previous year

            // don't attempt to get too much data (too wide a time span between the from and to parameters 
            // or the server will return a 500 Internal Server Error

            // the from time is the earliest time before which we are not interested
            // have to either hard-code this (currently 1-jan-2015)
            // or get it from the server (but there is no obvious way to get the earliest measurement timestamp for a given tag)

            // Need to split the time interval into chunks smaller chunks, for example a year at a time
            // need to balance between calling too often and spamming the server with too many requests
            // WirelessTagClientLib.HttpStatusException: "You have downloaded full log less than 30 seconds ago. Please configure URL call back to receive real-time tag data instead of polling the API GetTemperatureRawData"

            // also, the server does not like a request generating a huge amount of data in the respons:
            // if there are too many measurements in a time span, the server will return a 500 Internal Server Error
            // "Error during serialization or deserialization using the JSON JavaScriptSerializer. The length of the string exceeds the value set on the maxJsonLength property"


            //var today = DateTime.Now.Date;
            //var to = today.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // end of yesterday

            bool finished = false;

            var from = new DateTime(baseline.Year, 1, 1, 0, 0, 0); // start of the first day of the year
            var to = new DateTime(baseline.Year, 12, 31, 23, 59, 59); // end of the last day of the year

            while (!finished)
            {
                foreach (var tag in tags.Take(1))
                {
                    var tagId = tag.SlaveId;

                    // this call will be quite slow as it can return many thousands of data points
                    var data = await client.GetTemperatureRawDataAsync(tagId, from, to);

                    // back on UI thread
                    Console.WriteLine($"Tag {tagId} : Read {data.Count} data points");

                    viewModel.RawDataCache.Update(tagId, data);

                    // get all the data in the cache
                    var rawData = viewModel.RawDataCache.GetData(tagId);

                    Console.WriteLine($"Tag {tagId} : Cached {rawData.Count()} data points");

                    UpdateViewModel(viewModel, tag, data, false);
                }

                // move to the next year
                from = from.AddYears(1);
                to = to.AddYears(1);

                if (from.Year > DateTime.Now.Year)
                {
                    // no more data to get
                    finished = true;
                }
            }


            //foreach (var tag in tags)
            //{
            //    var tagId = tag.SlaveId;

            //    if (!viewModel.RawDataCache.ContainsDataForTag(tagId, from))
            //    {
            //        // this call will be quite slow as it returns tens of thousands of data points
            //        var data = await client.GetTemperatureRawDataAsync(tagId, from, to);

            //        // back on UI thread
            //        Console.WriteLine($"Tag {tagId} : Read {data.Count} data points");

            //        viewModel.RawDataCache.Update(tagId, data);

            //        // get all the data in the cache
            //        var rawData = viewModel.RawDataCache.GetData(tagId);

            //        Console.WriteLine($"Tag {tagId} : Cached {rawData.Count()} data points");

            //        UpdateViewModel(viewModel, tag, data, false);
            //    }
            //}

            return;
        }

        private async Task GetTodayDataForTags(MinMaxViewModel viewModel, List<TagInfo> tags)
        {
            var today = DateTime.Now.Date;
            var from = today.Date;
            var to = today.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // end of today

            foreach (var tag in tags)
            {
                var tagId = tag.SlaveId;

                // remove any data for today that is already present        
                var itemsToRemove = viewModel.Data.Where(d => d.TagId == tagId && d.Interval == TimeInterval.Today).ToList();

                if (itemsToRemove.Any())
                {
                    Console.WriteLine($"Tag {tagId} : Already has data for interval {from} to {to}; removing");

                    foreach (var item in itemsToRemove)
                    {
                        viewModel.Data.Remove(item);
                    }
                }

                var data = await client.GetTemperatureRawDataAsync(tagId, from, to);

                // back on UI thread
                viewModel.RawDataCache.Update(tagId, data);

                UpdateViewModel(viewModel, tag, data, true);
            }

            return;
        }

        /// <summary>
        /// Get recent historical data (1st January this year through to end of yesterday)
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private async Task GetRecentDataForTags(MinMaxViewModel viewModel, List<TagInfo> tags)
        {
            var today = DateTime.Now.Date;
            var from = new DateTime(today.Year, 1, 1); // start of year
            var to = today.Date.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // end of yesterday

            foreach (var tag in tags)
            {
                var tagId = tag.SlaveId;

                if (viewModel.RawDataCache.ContainsDataForTag(tagId, from, to))
                {
                    // already has data for this time period
                    Console.WriteLine($"Tag {tagId} : Already has data for interval {from} to {to}; skipping");
                    continue;
                }

                var data = await client.GetTemperatureRawDataAsync(tagId, from, to);

                if (data != null)
                {
                    var outsideData = data.Where(d => d.Time < from || d.Time > to).ToList();

                    if (outsideData.Any())
                    {
                        Console.WriteLine($"Tag {tagId} : Found data outside interval {from} to {to}");

                        // log the data points that are outside the time interval
                        var outsideDataCsv = CreatCSVData(outsideData);
                        File.WriteAllText($@"c:\temp\Tag-{tagId}-OutsideInterval.csv", outsideDataCsv);
                    }
                }

                // back on UI thread
                viewModel.RawDataCache.Update(tagId, data);

                UpdateViewModel(viewModel, tag, data, false);
            }

            return;
        }


        private void UpdateViewModel(MinMaxViewModel viewModel, TagInfo tag, List<TemperatureDataPoint> data, bool todayOnly)
        {
            var tagId = tag.SlaveId;

            // back on UI thread
            viewModel.RawDataCache.Update(tagId, data);

            // get all the data in the cache
            var rawData = viewModel.RawDataCache.GetData(tagId);

            if (data == null || !data.Any())
            {
                Console.WriteLine($"Tag {tagId} : *No data*");
            }
            else
            {
                Console.WriteLine($"Tag {tagId} : Read {data.Count} data points");
            }

            // split raw data for current tag into time ranges (for example last 7 days)
            IEnumerable<TimeInterval> timeIntervals;
            if (todayOnly)
            {
                timeIntervals = new[] { TimeInterval.Today };
            }
            else
            {
                timeIntervals = new[] { TimeInterval.Yesterday, TimeInterval.Last7Days, TimeInterval.Last30Days, TimeInterval.ThisYear, TimeInterval.All };
            }

            foreach (var interval in timeIntervals)
            {
                var rowViewModel = ViewModelFactory.CreateRowViewModel(rawData, tag, interval);

                // some tags may not have any data points in the time period of interest
                if (rowViewModel != null) // null means no data within the time interval
                {
                    viewModel.Data.Add(rowViewModel);
                }
            }
        }

        private string CreatCSVData(IEnumerable<TemperatureDataPoint> dataPoints)
        {
            var writer = new CSVWriter<TemperatureDataPoint>();
            writer.AddColumn(x => x.Temperature.ToString("f1"), "Temperature");
            writer.AddColumn(x => x.Time.ToString("dd-MM-yyyy HH:mm:ss"), "Timestamp");

            return writer.WriteCSV(dataPoints.ToList());
        }
    }
 }
