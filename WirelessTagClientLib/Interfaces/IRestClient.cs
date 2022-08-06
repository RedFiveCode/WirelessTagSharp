using RestSharp;
using System.Threading.Tasks;

namespace WirelessTagClientLib
{
    public interface IRestClient
    {
        RestResponse Execute(RestRequest request);

        Task<RestResponse> ExecuteAsync(RestRequest request);
    }
}
