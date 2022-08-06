using CommandLine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientConsole
{
    class Program
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
                        //SimpleJsonClient(o);


                        //
                        // simple synchronous client returning strongly typed DTO objects
                        //
                        //SimpleClient(o);


                        //
                        // simple asynchronous client returning strongly typed DTO objects
                        //

                        AsyncClient(o);
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

            var tagList = client.GetTagListAsync();

            Task.WaitAll(tagList);

            Console.WriteLine($"{tagList.Status}");

            Console.WriteLine($"{tagList.Result.Count} tag(s):");

            foreach (var tag in tagList.Result.OrderByDescending(t => t.LastCommunication))
            {
                Console.WriteLine($"  {tag.SlaveId} ({tag.Uuid}) ({tag.TagType}) : {tag.Name} : {tag.LastCommunication}, Temperature {tag.Temperature:n1} C, Humidity {tag.RelativeHumidity:n0} %");
            }

            Console.WriteLine("Today:");

            foreach (var tag in tagList.Result)
            {
                var infoList = client.GetTemperatureRawDataAsync(tag.SlaveId, DateTime.Today, DateTime.Today);
                Task.WaitAll(infoList);

                if (infoList.Result.Count > 0)
                {
                    Console.WriteLine($"  Tag {tag.SlaveId} ({tag.Name}) : Min: {infoList.Result.Min(r => r.Temperature):n1}, Max: {infoList.Result.Max(r => r.Temperature):n1} C of {infoList.Result.Count} readings");
                }
                else
                {
                    // some tag may not have results for today, for example if they are broken and have not sent any data recently
                }
            }

            // send a request for each tag and wait for all responses to return
            var pendingResults = new List<Task<List<TemperatureDataPoint>>>();

            foreach (var tag in tagList.Result)
            {
                var infoList = client.GetTemperatureRawDataAsync(tag.SlaveId, DateTime.Today, DateTime.Today);

                pendingResults.Add(infoList);
            }

            Task.WaitAll(pendingResults.ToArray());

            foreach (var item in pendingResults)
            {
                // TODO - correlate response to request
                var readings = item.Result;

                if (readings.Count > 0)
                {
                    Console.WriteLine($"  Min: {readings.Min(r => r.Temperature):n1}, Max: {readings.Max(r => r.Temperature):n1} C of {readings.Count} readings");
                }
            }
        }
    }
}
