using RestSharp;
using System;

namespace WirelessTagClientLib.Requests
{
    /// <summary>
    /// Encapsulates a Json Post request
    /// </summary>
    internal abstract class JsonPostRequest
    {
        protected string Url = "https://www.mytaglist.com";

        private RestRequest request;

        protected JsonPostRequest(string endpoint)
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException(endpoint);
            }

            var resource = Url + endpoint;

            request = new RestRequest(endpoint, Method.Post);
            //request = new RestRequest(resource, Method.Post);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-Type", "application/json");
        }

        protected abstract object GetRequestBody();

        public RestRequest AsRestRequest
        {
            get
            {
                object body = GetRequestBody();
                request.AddBody(body);

                return request;
            }
        }

    }
}
