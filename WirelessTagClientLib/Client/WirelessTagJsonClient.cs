using System;
using System.Collections.Generic;
using System.Net;
using WirelessTagClientLib.Requests;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Synchronous client returning json string responses
    /// </summary>
    public class WirelessTagJsonClient : IWirelessTagJsonClient
    {
        private readonly IRestClient _client;

        public WirelessTagJsonClient(string accessToken)
        {
            Url = WirelessTagConstants.Url;
            _client = new RestClientWrapper(Url, accessToken);
        }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        /// <param name="restClient"></param>
        public WirelessTagJsonClient(IRestClient restClient)
        {
            _client = restClient;
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
        public string Login(string email, string password)
        {
            var request = new LoginRequest()
            {
                Email = email,
                Password = password
            };

            var response = _client.Execute(request.AsRestRequest);

            // get InternalServerError (500) if Email or password is incorrect
            return response.Content;
        }

        /// <summary>
        /// Returns true if logged in.
        /// </summary>
        /// <returns></returns>
        public string IsLoggedIn()
        {
            var request = new IsSignedInRequest();

            return Execute(request);
        }

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        public string GetTagList()
        {
            var request = new GetTagListRequest();

            return Execute(request);
        }

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public string GetTemperatureStats(int tagId)
        {
            var request = new GetTemperatureStatsRequest()
            {
                Id = tagId
            };

            return Execute(request);
        }

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public string GetTagSpanStats(List<int> tagIds)
        {
            var request = new GetMultiTagStatsSpanRequest()
            {
                Ids = tagIds
            };

            return Execute(request);
        }

        /// <summary>
        /// Get the raw data for a date range for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public string GetTemperatureRawData(int tagId, DateTime from, DateTime to)
        {
            var request = new GetTemperatureRawDataRequest()
            {
                Id = tagId,
                From = from,
                To = to
            };

            return Execute(request);
        }

        /// <summary>
        /// Get the raw temperature/battery/humidity data for the specified tag for a date range
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public string GetTagStatsRawData(int tagId, DateTime from, DateTime to)
        {
            var request = new GetStatsRawDataRequest()
            {
                Id = tagId,
                From = from,
                To = to
            };

            return Execute(request);
        }

        /// <summary>
        /// Execute the request, returning Json response
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Response json string</returns>
        /// <exception cref="HttpStatusException"></exception>
        private string Execute(JsonPostRequest request)
        {
            var response = _client.Execute(request.AsRestRequest);

            if (response.StatusCode != HttpStatusCode.OK) // 200
            {
                throw new HttpStatusException(response.StatusCode, response.Content);
            }
            return response.Content;
        }



    }
}
