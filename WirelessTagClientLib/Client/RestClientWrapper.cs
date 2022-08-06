using RestSharp;
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

        public RestClientWrapper(string url)
        {
            client = new RestClient(url);
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
