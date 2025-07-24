using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System.IO;
using System.Net;

namespace WirelessTagClientLib.Test.TestHelpers
{
    internal static class TestHelper
    {
        public static RestResponse GetRestResponseFromFile(string deploymentDirectory, string filename, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var data = GetResponseDataFromFile(deploymentDirectory, filename);

            var response = new RestResponse()
            {
                StatusCode = statusCode,
                Content = data,
                ContentType = "application/json"
            };

            return response;
        }

        public static string GetResponseDataFromFile(string deploymentDirectory, string filename)
        {
            var dataFile = Path.Combine(deploymentDirectory, "TestData", filename);

            Assert.IsTrue(File.Exists(dataFile), "Did you forget to add a [DeploymentItem] attribute or set the file's Build Action to 'None' and Copy 'Always'?");

            return File.ReadAllText(dataFile);
        }
    }
}
