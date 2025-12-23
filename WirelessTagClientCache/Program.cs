using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using WirelessTagClientLib;
using WirelessTagClientLib.Client;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientCache
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default
                        .ParseArguments<Options, MergeOptions, ShowOptions>(args)
                        .MapResult(
                            (Options opts) => CacheWrite(opts),
                            (MergeOptions opts) => CacheMerge(opts),
                            (ShowOptions opts) => CacheRead(opts),
                            errs =>
                            {
                                foreach (var error in errs)
                                {
                                    Console.WriteLine(error.ToString());
                                }
                                return Task.CompletedTask;
                            });
        }


        static async Task CacheWrite(Options options)
        {
            if (options.From > options.To)
            {
                Console.WriteLine("Error: 'from' date must be earlier than or equal to 'to' date.");
                Environment.Exit(1);
            }

            if (!Directory.Exists(options.Folder))
            {
                Console.WriteLine($"Error: The folder '{options.Folder}' does not exist.");
                Environment.Exit(1);
            }

            //Console.WriteLine($"Folder: {options.Folder}");
            //Console.WriteLine($"Tag Id: {options.TagId}");
            //Console.WriteLine($"From: {options.From:dd-MMM-yyyy}");
            //Console.WriteLine($"To: {options.To:dd-MMM-yyyy}");

            var client = new WirelessTagAsyncClient(options.AccessToken);

            var loader = new CacheWriter(client)
            {
                Verbose = options.Verbose,
                ChunkInterval = options.ChunkSize > 0 ? TimeSpan.FromDays(options.ChunkSize) : TimeSpan.FromDays(100),
                WaitInterval = options.WaitInterval > 0 ? TimeSpan.FromSeconds(options.WaitInterval) : TimeSpan.FromSeconds(30)
            };

            await loader.LoadCacheAsync(options.TagId, options.Folder, options.From, options.To);

            Console.WriteLine("Cache loading completed successfully.");

            // TODO try-catch
            // TODO colour output
        }

        static async Task CacheMerge(MergeOptions options)
        {
            if (!Directory.Exists(options.Folder))
            {
                Console.WriteLine($"Error: The folder '{options.Folder}' does not exist.");
                Environment.Exit(1);
            }

            var client = new WirelessTagAsyncClient(options.AccessToken);

            // get list of tags from the server, so can get the id and hence filename
            var tags = await client.GetTagListAsync();

            var tag = tags?.FirstOrDefault(t => t.SlaveId == options.TagId);

            if (tag == null)
            {
                Console.WriteLine($"Tag Id {options.TagId} not found");
                Environment.Exit(1);
            }

            var reader = new CacheFileReaderWriter();
            var cacheFile = reader.GetCacheFilename(options.Folder, tag);
            var cachedData = reader.ReadCacheFile(cacheFile);

            if (cachedData == null || !cachedData.Any())
            {
                Console.WriteLine($"Cache not found or empty for Tag Id {options.TagId}");
                Environment.Exit(1);
            }

            DisplayCacheStats(tag, cacheFile, cachedData);
            Console.WriteLine();

            var latest = cachedData.Max(m => m.Time);
            var from = latest.AddSeconds(1); // start just after the latest cached measurement
            var to = options.To;

            var loader = new CacheWriter(client)
            {
                Verbose = options.Verbose,
                ChunkInterval = options.ChunkSize > 0 ? TimeSpan.FromDays(options.ChunkSize) : TimeSpan.FromDays(100),
                WaitInterval = options.WaitInterval > 0 ? TimeSpan.FromSeconds(options.WaitInterval) : TimeSpan.FromSeconds(30)
            };

            await loader.UpdateCacheAsync(options.TagId, options.Folder, from, to);


            cachedData = reader.ReadCacheFile(cacheFile);

            if (cachedData == null || !cachedData.Any())
            {
                Console.WriteLine($"Cache not found or empty for Tag Id {options.TagId}");
                Environment.Exit(1);
            }

            // show updated cache stats
            Console.WriteLine();
            cachedData = reader.ReadCacheFile(cacheFile);
            DisplayCacheStats(tag, cacheFile, cachedData);
            ColorConsole.WriteLine("Cache merging completed successfully", ConsoleColor.White);
        }

        private static void DisplayCacheStats(TagInfo tag, string cacheFile, List<Measurement> cachedData)
        {
            var earliest = cachedData.Min(m => m.Time);
            var latest = cachedData.Max(m => m.Time);
            var cacheFileNoPath = Path.GetFileName(cacheFile);

            ColorConsole.WriteLine($"Cached data:", ConsoleColor.White);
            ColorConsole.Write($"{earliest}  {latest}  {cachedData.Count,7:N0}  ", ConsoleColor.Green);
            ColorConsole.Write($"{cacheFileNoPath}  ", ConsoleColor.Blue);
            ColorConsole.WriteLine($"{tag.SlaveId} ({tag.Name})", ConsoleColor.Yellow);
        }

        private static async Task CacheRead(ShowOptions options)
        {
            if (!Directory.Exists(options.Folder))
            {
                Console.WriteLine($"Error: The folder '{options.Folder}' does not exist.");
                Environment.Exit(1);
            }

            var reader = new CacheFileReaderWriter();

            var client = new WirelessTagAsyncClient(options.AccessToken);

            // get list of tags from the server, so can get the id and hence filename
            var tags = await client.GetTagListAsync();

            if (tags == null || tags.Count == 0)
            {
                Console.WriteLine("No tags found");
                return;
            }

            foreach (var tag in tags)
            {
                var cacheFile = reader.GetCacheFilename(options.Folder, tag);
                var data = reader.ReadCacheFile(cacheFile);

                if (data != null && data.Any()) // skip cache files with no data
                {
                    var earliest = data.Min(m => m.Time);
                    var latest = data.Max(m => m.Time);
                    var cacheFileNoPath = Path.GetFileName(cacheFile);

                    ColorConsole.Write($"{earliest}  {latest}  {data.Count,7:N0}  ", ConsoleColor.Green);
                    ColorConsole.Write($"{cacheFileNoPath}  ", ConsoleColor.Blue);
                    ColorConsole.WriteLine($"{tag.SlaveId} ({tag.Name})", ConsoleColor.Yellow);
                }
            }
        }
    }

}
