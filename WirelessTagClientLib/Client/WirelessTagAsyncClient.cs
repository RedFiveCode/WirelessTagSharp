using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WirelessTagClientLib.DTO;
using WirelessTagClientLib.RawResponses;
using WirelessTagClientLib.Requests;

namespace WirelessTagClientLib
{

    /// <summary>
    /// Synchronous client with strongly typed responses
    /// </summary>
    public class WirelessTagAsyncClient : IWirelessTagAsyncClient
    {
        private readonly IRestClient client;

        public WirelessTagAsyncClient(string accessToken)
        {
            Url = WirelessTagConstants.Url;
            client = new RestClientWrapper(Url, accessToken);
        }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        /// <param name="restClient"></param>
        public WirelessTagAsyncClient(IRestClient restClient)
        {
            client = restClient;
            Url = WirelessTagConstants.Url;
        }

        public string Url { get; private set; }


        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="password">Password.</param>
        public async Task<bool> LoginAsync(string email, string password)
        {
            var request = new LoginRequest()
            {
                Email = email,
                Password = password
            };

            var response = await client.ExecuteAsync(request.AsRestRequest);

            // get InternalServerError (500) if Email or password is incorrect
            return response != null && response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        public async Task<List<TagInfo>> GetTagListAsync()
        {
            var request = new GetTagListRequest();

            var response = await ExecuteAsync(request.AsRestRequest);

            var payload = JsonConvert.DeserializeObject<TagListResponse>(response.Content);
            return Mapper.Create(payload);
        }

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<HourlyReadingInfo> GetTemperatureStatsAsync(int tagId)
        {
            var request = new GetTemperatureStatsRequest()
            {
                Id = tagId
            };

            var response = await ExecuteAsync(request.AsRestRequest);

            var payload = JsonConvert.DeserializeObject<TemperatureStatsResponse>(response.Content);
            return Mapper.Create(payload);
        }

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public async Task<StatsSpanInfo> GetTagSpanStatsAsync(List<int> tagIds)
        {
            var request = new GetMultiTagStatsSpanRequest()
            {
                Ids = tagIds
            };

            var response = await ExecuteAsync(request.AsRestRequest);

            var payload = JsonConvert.DeserializeObject<MultiTagStatsSpanResponse>(response.Content);
            return Mapper.Create(payload.Data);
        }


        /// <summary>
        /// Get the raw data for a date range for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public async Task<List<TemperatureDataPoint>> GetTemperatureRawDataAsync(int tagId, DateTime from, DateTime to)
        {
            var request = new GetTemperatureRawDataRequest()
            {
                Id = tagId,
                From = from,
                To = to
            };

            var response = await ExecuteAsync(request.AsRestRequest);

            var payload = JsonConvert.DeserializeObject<TemperatureRawDataResponse>(response.Content);
            return Mapper.Create(payload);
        }

        private async Task<RestSharp.RestResponse> ExecuteAsync(RestSharp.RestRequest request)
        {
            var response = await client.ExecuteAsync(request);

            if (response == null)
            {
                throw new HttpStatusException(HttpStatusCode.NoContent);
            }

            // will throw exception here if the request times out or has an error
            if (response.StatusCode != HttpStatusCode.OK) // 200
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.Content}");
                throw new HttpStatusException(response.StatusCode, response.Content);
            }

            return response;
        }
    }
}
