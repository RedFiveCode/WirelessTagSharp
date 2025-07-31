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
    public class CacheLoader
    {
        private readonly IWirelessTagAsyncClient _client;
        private readonly IFileSystem _fileSystem;

        public CacheLoader(IWirelessTagAsyncClient client) : this(client, new FileSystem())
        { }

        public CacheLoader(IWirelessTagAsyncClient client, IFileSystem fileSystem)
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

        public bool Verbose { get; set; } = false;

        public async Task LoadCacheAsync(int tagId, string folder, DateTime from, DateTime to)
        {
            // TODO ThrowIf.Argument.IsNull(folder, nameof(folder));

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

            using (var stream = _fileSystem.FileStream.New(filename, FileMode.CreateNew))
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
