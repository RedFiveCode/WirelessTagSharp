using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using WirelessTagClientLib.DTO;
using WirelessTagClientLib.RawResponses;
using WirelessTagClientLib.Requests;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Synchronous client with strongly typed responses
    /// </summary>
    public class WirelessTagClient : IWirelessTagClient
    {
        private IRestClient client;

        public WirelessTagClient(string accessToken)
        {
            Url = WirelessTagConstants.Url;
            client = new RestClientWrapper(Url, accessToken);
        }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        /// <param name="restClient"></param>
        public WirelessTagClient(IRestClient restClient)
        {
            client = restClient;
            Url = WirelessTagConstants.Url;
        }

        public string Url { get; private set; }


        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Login gets us an authentication cookie for use with subsequent requests
        /// </remarks>
        /// <param name="email">Email address.</param>
        /// <param name="password">Password.</param>
        public bool Login(string email, string password)
        {
            var request = new LoginRequest()
            {
                Email = email,
                Password = password
            };

            var response = client.Execute(request.AsRestRequest);

            // get InternalServerError (500) if Email or password is incorrect
            return response != null && response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        public List<TagInfo> GetTagList()
        {
            var request = new GetTagListRequest();

            var response = Execute(request);
            var payload = JsonConvert.DeserializeObject<TagListResponse>(response);
            return Mapper.Create(payload);
        }

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public HourlyReadingInfo GetTemperatureStats(int tagId)
        {
            var request = new GetTemperatureStatsRequest()
            {
                Id = tagId
            };

            var response = Execute(request);
            var payload = JsonConvert.DeserializeObject<TemperatureStatsResponse>(response);
            return Mapper.Create(payload);
        }

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public StatsSpanInfo GetTagSpanStats(List<int> tagIds)
        {
            var request = new GetMultiTagStatsSpanRequest()
            {
                Ids = tagIds
            };

            var response = Execute(request);
            var payload = JsonConvert.DeserializeObject<MultiTagStatsSpanResponse>(response);
            return Mapper.Create(payload.Data);
        }

        /// <summary>
        /// Get the raw data for a date range for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<TemperatureDataPoint> GetTemperatureRawData(int tagId, DateTime from, DateTime to)
        {
            var request = new GetTemperatureRawDataRequest()
            {
                Id = tagId,
                From = from,
                To = to
            };

            var response = Execute(request);
            var payload = JsonConvert.DeserializeObject<TemperatureRawDataResponse>(response);
            return Mapper.Create(payload);
        }

        /// <summary>
        /// Execute the request, returning Json response
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response json string</returns>
        /// <exception cref="HttpStatusException"></exception>
        private string Execute(JsonPostRequest request)
        {
            var response = client.Execute(request.AsRestRequest);

            if (response == null)
            {
                throw new HttpStatusException(HttpStatusCode.NoContent);
            }

            if (response.StatusCode != HttpStatusCode.OK) // 200
            {
                throw new HttpStatusException(response.StatusCode, response.Content);
            }
            return response.Content;
        }

        private string ExecuteNoThrow(JsonPostRequest request)
        {
            var response = client.Execute(request.AsRestRequest);

            return response.Content;
        }
    }
}