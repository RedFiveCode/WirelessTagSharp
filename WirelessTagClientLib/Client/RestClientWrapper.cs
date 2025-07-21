using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Threading.Tasks;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Wrapper around RestSharp RestClient to make it unit testable
    /// </summary>
    internal class RestClientWrapper : IRestClient
    {
        private readonly RestClient _client;

        public RestClientWrapper()
        {
            _client = new RestClient();
        }

        public RestClientWrapper(string url, string accessToken) : this(url, accessToken, TimeSpan.FromSeconds(120))
        { }

        public RestClientWrapper(string url, string accessToken, TimeSpan requestTimeout)
        {
            var options = new RestClientOptions(url)
            {
                Timeout = requestTimeout,
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer")
            };

            _client = new RestClient(options);
        }

        public RestClientWrapper(RestClient client)
        {
            this._client = client;
        }

        public RestResponse Execute(RestRequest request)
        {
            return _client.Execute(request);
        }

        public Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return _client.ExecuteAsync(request);
        }
    }
}
