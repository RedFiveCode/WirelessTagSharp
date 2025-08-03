using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib.Client
{
    /// <summary>
    /// CacheLoader is responsible for getting raw temperature measurement data from a Wireless Tag client
    /// in a controlled way to avoid overloading the server with requests and writing the data to a cache file for each tag.
    /// </summary>
    public class CacheWriter
    {
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
        // "Error during serialization or deserialiszation using the JSON JavaScript
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
        //
        // If we are caching the data to a file as an occasional activity,
        // we can afford to wait a bit longer for the data to be fetched...
        //

        private readonly IWirelessTagAsyncClient _client;
        private readonly IFileSystem _fileSystem;

        public CacheWriter(IWirelessTagAsyncClient client) : this(client, new FileSystem())
        { }

        public CacheWriter(IWirelessTagAsyncClient client, IFileSystem fileSystem)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client), "Client cannot be null");
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem), "FileSystem cannot be null");
        }

        /// <summary>
        /// Time to wait between sending requests.
        /// </summary>
        public TimeSpan WaitInterval { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Time interval for fetching data in chunks.
        /// </summary>
        public TimeSpan ChunkInterval { get; set; } = TimeSpan.FromDays(100);

        /// <summary>
        /// Optional verbose console output.
        /// </summary>
        public bool Verbose { get; set; } = false;

        /// <summary>
        /// Load the cache for a specific tag and time range.
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="folder">Cache folder name</param>
        /// <param name="from">Start of time interval</param>
        /// <param name="to">End of time interval (inclusive)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="folder"/> is null</exception>"
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="folder"/></exception>
        public async Task LoadCacheAsync(int tagId, string folder, DateTime from, DateTime to)
        {
            ThrowIf.Argument.IsNull(folder, nameof(folder));

            // Load tags from the client
            var tagInfo = await GetTagInfoAsync(tagId);
            if (tagInfo == null)
            {
                throw new ArgumentOutOfRangeException($"Tag with Id {tagId} not found.");
            }

            var measurements = new List<Measurement>();

            // split time range into smaller chunks starting at midnight of the beginning of the specified date range
            var start = from.Date;
            var finish = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var timechunks = DateTimeChunker.SplitDateTimeRange(from.Date, to.Date, ChunkInterval);

            Console.WriteLine($"From {start} to {finish} in {timechunks.Count()} chunks of {ChunkInterval.TotalDays} days...");

            foreach (var chunk in timechunks)
            {
                if (Verbose)
                {
                    Console.Write($"Tag {tagId} : Getting data from {chunk.Start} to {chunk.End}...");
                }

                // fetch the data from the client
                var stopwatch = Stopwatch.StartNew();
                var data = await _client.GetTemperatureRawDataAsync(tagId, chunk.Start, chunk.End);
                stopwatch.Stop();

                if (data != null && data.Any())
                {
                    if (Verbose)
                    {
                        Console.WriteLine($"{data.Count:n0} measurements in {stopwatch.Elapsed}");
                    }

                    // Add the fetched data to the measurements list
                    measurements.AddRange(data);
                }
                else
                {
                    if (Verbose)
                    {
                        Console.WriteLine("no data");
                    }
                }

                // wait between requests unless the last chunk
                if (chunk != timechunks.Last())
                {
                    // Wait for a specified time before the next request
                    if (Verbose)
                    {
                        Console.Write($"Waiting ({WaitInterval})...");
                    }

                    await Task.Delay(WaitInterval);

                    if (Verbose)
                    {
                        Console.WriteLine("ok");
                    }
                }
            }

            // Serialize and save the data to the cache file
            if (measurements.Any())
            {
                // Ensure the folder exists
                if (!_fileSystem.Directory.Exists(folder))
                {
                    _fileSystem.Directory.CreateDirectory(folder);
                }

                var filename = GetCacheFilename(folder, tagInfo);

                if (Verbose)
                {
                    Console.WriteLine($"Writing {measurements.Count:n0} measurements to {filename}...");
                }

                WriteCacheFile(filename, measurements);
            }
        }

        private async Task<TagInfo> GetTagInfoAsync(int tagId)
        {
            var tags = await _client.GetTagListAsync();

            // Find the tag with the specified ID
            return tags.FirstOrDefault(tag => tag.SlaveId == tagId);
        }

        private string GetCacheFilename(string folder, TagInfo tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null");
            }

            // Create a filename based on the tag's UUID and the date range
            var filename = $"{tag.Uuid}.cache.json";

            return Path.Combine(folder, filename);
        }

        private void WriteCacheFile(string filename, List<Measurement> data)
        {
            var serializer = new JsonSerializer()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.Indented
            };

            using (var stream = _fileSystem.FileStream.New(filename, FileMode.Create, FileAccess.Write))
            {
                using (var tw = new StreamWriter(stream, Encoding.UTF8))
                {
                    // Serialize the data to the file
                    using (JsonWriter writer = new JsonTextWriter(tw))
                    {
                        serializer.Serialize(writer, data);
                    }
                }
            }
        }
    }
}
