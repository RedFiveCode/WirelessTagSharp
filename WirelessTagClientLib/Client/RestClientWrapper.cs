using RestSharp;
using RestSharp.Authenticators;
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
        private RestClient client;

        public RestClientWrapper()
        {
            client = new RestClient();
        }

        public RestClientWrapper(string url, string accessToken) : this(url, accessToken, TimeSpan.FromSeconds(120))
        { }

        public RestClientWrapper(string url, string accessToken, TimeSpan requestTimeout)
        {
            var options = new RestClientOptions(url)
            {
                MaxTimeout = (int)requestTimeout.TotalMilliseconds,
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer")
            };

            client = new RestClient(options);
        }

        public RestClientWrapper(RestClient client)
        {
            this.client = client;
        }

        public RestResponse Execute(RestRequest request)
        {
            return client.Execute(request);
        }

        public Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return client.ExecuteAsync(request);
        }
    }
}
