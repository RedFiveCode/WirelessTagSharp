using CommandLine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // username and password are set on the command line: "/u user /p password"
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(o =>
                    {
                        //
                        // simple synchronous client returning json strings
                        //
                        SimpleJsonClient(o);


                        //
                        // simple synchronous client returning strongly typed DTO objects
                        //
                        //SimpleClient(o);


                        //
                        // simple asynchronous client returning strongly typed DTO objects
                        //

                        //AsyncClient(o);
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message} at\n{ex.StackTrace}");
            }
        }



        private static void SimpleJsonClient(Options options)
        {
            //
            // simple synchronous client returning json strings
            //

            var client = new WirelessTagJsonClient();

            var response = client.Login(options.Username, options.Password);
            var loggedIn = client.IsLoggedIn();

            var tagList = client.GetTagList();
            Console.WriteLine(tagList);

            var tagSpans = client.GetTagSpanStats(new List<int> { 1, 2, 3 });
            Console.WriteLine(tagSpans);

            var temperatureStats = client.GetTemperatureStats(1);
            Console.WriteLine(temperatureStats);

            var data = client.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);
            Console.WriteLine(data);

            var rawStatsData = client.GetTagStatsRawData(1, DateTime.Today.AddDays(-1), DateTime.Today);
            Console.WriteLine(rawStatsData);
        }

        private static void SimpleClient(Options options)
        {
            //
            // simple synchronous client returning strongly typed DTO objects
            //
            var client = new WirelessTagClient();
            bool loggedIn = client.Login(options.Username, options.Password);

            var tagList = client.GetTagList();

            Console.WriteLine($"{tagList.Count} tag(s):");

            foreach (var tag in tagList.OrderByDescending(t => t.LastCommunication))
            {
                Console.WriteLine($"  {tag.SlaveId} ({tag.Uuid}) ({tag.TagType}) : {tag.Name} : {tag.LastCommunication}, Temperature {tag.Temperature:n1} C, Humidity {tag.RelativeHumidity:n0} %");
            }

            var tagSpans = client.GetTagSpanStats(new List<int> { 1, 2, 3 });

            var temperatureStats = client.GetTemperatureStats(1);

            var data = client.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);
        }

        private static void AsyncClient(Options options)
        {
            //
            // simple asynchronous client returning strongly typed DTO objects
            //
            var client = new WirelessTagAsyncClient();

            Console.WriteLine($"Connecting to {client.Url}...");

            var loggedIn = client.LoginAsync(options.Username, options.Password);
            Task.WaitAll(loggedIn);

            if (!loggedIn.Result)
            {
                Console.WriteLine($"Unable to connect to {client.Url}");
                return;
            }

            var tagListTask = client.GetTagListAsync();

            Task.WaitAll(tagListTask);

            Console.WriteLine($"{tagListTask.Status}");

            var tagList = tagListTask.Result;

            Console.WriteLine($"{tagList.Count} tag(s):");

            foreach (var tag in tagList.OrderByDescending(t => t.LastCommunication))
            {
                Console.WriteLine($"  {tag.SlaveId} ({tag.Uuid}) ({tag.TagType}) : {tag.Name} : {tag.LastCommunication}, Temperature {tag.Temperature:n1} C, Humidity {tag.RelativeHumidity:n0} %");
            }

            var today = DateTime.Today;

            DisplayRecentTemperatures(client, "Today:", tagList, today, today);

            DisplayRecentTemperatures(client, "This week:", tagList, today.AddDays(-7), today);

            DisplayRecentTemperatures(client, "This month:", tagList, today.AddDays(-today.Day + 1), DateTime.Today);

            DisplayRecentTemperatures(client, "This year:", tagList, today.AddDays(-today.DayOfYear + 1), DateTime.Today);
         }

        private static void DisplayRecentTemperatures(IWirelessTagAsyncClient client, string caption, List<TagInfo> tags, DateTime from, DateTime to)
        {
            var endTime = to.AddDays(1).AddSeconds(-1); // 23:59:59 today

            Console.WriteLine($"{caption} ({from} to {endTime}):");

            var results = ClientUtils.GetRecentTemperatures(client, tags, from, to);

            // get tag name with longest length for pretty alignment
            var longestTagName = tags.Max(t => t.Name.Length);
            longestTagName += 2; // for leading and trailing brackets
            var fmt = "{0,-" + longestTagName.ToString() + "}"; // for example "{0,-12}"

            foreach (var result in results)
            {
                var tagName = "(" + result.Tag.Name + ")"; // want tag name in brackets then right padded with spaces to width of longest tag name

                Console.WriteLine("  {0} {1} : Min: {2,4:n1}, Max: {3,4:n1}, Current {4,4:n1} °C",
                                            result.Tag.SlaveId,
                                            tagName.PadRight(longestTagName, ' '),
                                            result.Min,
                                            result.Max,
                                            result.Tag.Temperature);
            }
        }
    }
}
