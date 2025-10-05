using AsyncAwaitBestPractices.MVVM;
using System.Diagnostics;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Extensions;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;


namespace WirelessTagClientApp.Commands
{
    public class RefreshMinMaxTagsCommand
    {
        private readonly IWirelessTagAsyncClient _client;
        private readonly ICacheFileReaderWriter _cacheReaderWriter;
        private readonly string _cacheFolder;

        private Dictionary<int, List<Measurement>> _cachedData;

        public IAsyncCommand<MinMaxViewModel> Command { get; private set; }

        public RefreshMinMaxTagsCommand(IWirelessTagAsyncClient client, ICacheFileReaderWriter cacheReaderWriter, Options options)
        {
            _client = client;
            _cacheReaderWriter = cacheReaderWriter;
            Command = new AsyncCommand<MinMaxViewModel>(p => ExecuteAsync(p), p => CanExecute(p));

            _cachedData = new Dictionary<int, List<Measurement>>();

            // get fully qualified cache folder name (or null if not specified or does not exist)
            var optionsExtensions = new OptionsExtensions();
            _cacheFolder = optionsExtensions.ResolveCacheFolder(options);
        }

        private bool CanExecute(object p)
        {
            return true;
        }

        public async Task ExecuteAsync(MinMaxViewModel viewModel)
        {
            try
            {
                // set busy overlay
                viewModel.ParentViewModel.IsBusy = true;

                // clear any previous results
                //viewModel.Data.Clear();

                var tagList = await _client.GetTagListAsync();

                if (tagList == null || !tagList.Any())
                {
                    // no tags
                    return;
                }

                viewModel.Data.Clear(); // clear min-max measurements
                foreach (var tag in tagList)
                {
                    await GetDataAndRefreshViewModelForTag(viewModel, tag);
                }

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

        private async Task GetDataAndRefreshViewModelForTag(MinMaxViewModel viewModel, TagInfo tag)
        {
            var baselineTimestamp = new DateTime(2015, 1, 1);

            // IMPORTANT
            //
            // Balances spamming the server with requests for many requests with short time intervals
            // and sending fewer requests with wider time intervals that generating too big a response on the server.
            //
            // We need to get historic temperature measurements for all the tags and a time span...
            //
            // We can create tasks/async await to send a request to get temperature data for a given tag and time span
            // and await each request/task to complete.
            //
            // However the are two problems:
            //
            // 1) The server does not like too many requests too quickly, so we need to control the rate of requests.
            // We hope to avoid too many concurrent requests or subsequent requests in too short a period
            // as this can result in HttpStatusException / HttpStatusCode.InternalServerError AT THE SERVER 
            // "You have downloaded full log less than 30 seconds ago
            //   Please configure URL call back to receive real-time tag _data instead of polling the API GetTemperatureRawData",
            //   "StackTrace":
            //   at MyTagList.ethLogs.CheckLogDownloadRate
            //   at MyTagList.ethLogs.CheckT
            //   at MyTagList.ethLogs.GetTemperatureRaw
            //   "ExceptionType":"MyTagList.ethLogs+TooManyRequest
            //
            // 2) The server does not like too big a response.
            // Too big an interval (for example getting 5 years of data in one request)
            // where there are too many measurements in a time span can result in
            // a further Server crash due to the size of the data attempted to be returned.
            // HttpStatusException / HttpStatusCode.InternalServerError AT THE SERVER 
            // "Error during serialization or deserialisation using the JSON JavaScript
            //  The length of the string exceeds the value set on the maxJsonLength property"
            //
            // So we have to find a workable balance between these two possible Server errors,
            // and also getting an acceptable wait while we get data.
            //
            // We can mitigate #1 by performing one query per tag to get all the temperature data over a long time.
            // The UI will then split this into chunks locally to show min and max for a given timespan
            // such as this week, this month, year to date, etc.
            // Since each query potentially takes a long time, this avoids the risk
            // of spamming the server with too many requests.
            // This approach does not address #2, and in all likelihood will push things closer to the server side
            // serialisation limit result, resulting in the Server 500 error.
            //
            // We can mitigate #2 by splitting the time range into smaller chunks,
            // but not making the queries too frequently (to avoid #1).
            //
            // A refinement of this approach is to recognise that the UI shows data for a specific time span
            // such as this week, this month, year to date, etc.
            // Data older than the start of the current year can be considered 'historic' as it does not contribute
            // to the min and max calculations for time intervals such as today, this week, this month, year to date, etc.
            // (ignoring edge effects in the first few days of a new year).
            //
            // The historic data contributes to the overall min and max calculations for a given time span
            // but since the data does not change,
            // we can safely fetch this data once and cache the data in a persistent store such as a file.
            //
            // Remember that some tags may not have any data points in the a given time period.
            // Ideally should call GetTagSpanStatsAsync/GetMultiTagStatsSpan to get time range for each tag.

            var tagId = tag.SlaveId;

            // use older data from cache; likely data from before this year
            if (!_cachedData.ContainsKey(tagId))
            {
                // assume if there is a key in the cache, then there is some data as well and not an empty list of measurements
                var data = GetCachedDataForTag(tag, _cacheFolder);
                _cachedData[tagId] = data; // if no cached data for this tag, then returns an empty list

                Console.WriteLine($"Tag {tagId} : Using {data.Count:N0} cached data points");
                viewModel.RawDataCache.Update(tagId, data);
            }

            // get most recent data point; if no data in the cache for the current tag, then returns null
            var latestMeasurement = viewModel.RawDataCache.GetLatestData(tagId);

            // Get data after the most recent data point
            // Requery data for today to get more recent measurements;
            // and get overlap/duplicate measurements. which the cache will resolve
            // if no data in the cache for the current tag, then use arbitrary starting date to query from
            var from = latestMeasurement != null ? latestMeasurement.Time : baselineTimestamp;

            var to = DateTime.Now.Date.RoundUp();

            var measurementsRecent = await GetDataForTag(tagId, from, to);

            viewModel.RawDataCache.Update(tagId, measurementsRecent);

            // update time interval buckets
            var timeIntervals = new[]
            {
                TimeInterval.Today,
                TimeInterval.Yesterday,
                TimeInterval.Last7Days,
                TimeInterval.Last30Days,
                TimeInterval.ThisYear,
                TimeInterval.All
            };

            var allMeasurementsForTag = viewModel.RawDataCache.GetData(tagId);

            foreach (var interval in timeIntervals)
            {
                var rowViewModel = ViewModelFactory.CreateRowViewModel(allMeasurementsForTag, tag, interval);

                // some tags may not have any measurements in the time interval of interest
                if (rowViewModel != null)
                {
                    viewModel.Data.Add(rowViewModel);
                }
            }
        }

        private async Task<List<Measurement>> GetDataForTag(int tagId, DateTime from, DateTime to)
        {
            Debug.WriteLine($"Tag {tagId} : Getting data for {from} to {to}...");

            var data = await _client.GetTemperatureRawDataAsync(tagId, from, to);

            if (data != null && data.Any())
            {
                Debug.WriteLine($"Tag {tagId} : Read {data.Count} data points");
            }
            else
            {
                Debug.WriteLine($"Tag {tagId} : *No data*");
            }

            return data;
        }

        private List<Measurement> GetCachedDataForTag(TagInfo tag, string cacheFolder)
        {
            // if cache folder is specified, read older data from before this year from cache
            if (String.IsNullOrEmpty(cacheFolder))
            {
                return new List<Measurement>(); // empty list
            }

            var cacheFile = _cacheReaderWriter.GetCacheFilename(cacheFolder, tag);
            var data = _cacheReaderWriter.ReadCacheFile(cacheFile);

            Debug.WriteLine($"Tag {tag.SlaveId} : Read {data.Count:N0} measurements from cache");

            return data;
        }
    }
 }
