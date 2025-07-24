using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WirelessTagClientLib.Test.TestHelpers;
using Xunit;

namespace WirelessTagClientLib.Test
{
    public class WirelessTagJsonClientTest
    {
        [Fact]
        public void Execute_Response_Not_Ok_Should_Throw_HttpStatusException()
        {
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Throws(new HttpStatusException(HttpStatusCode.InternalServerError, "Internal Server Error"));

            var target = new WirelessTagJsonClient(clientMock.Object);
            var request = new DummyRequest();

            Assert.Throws<HttpStatusException>(() => target.Login("user", "secret"));
        }

        [Fact]
        public void Execute_Response_Ok_Should_Return_Valid_Response()
        {
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(new RestResponse()
                       { 
                          StatusCode = HttpStatusCode.OK,
                          Content = "some response data",
                          ContentType = "text/plain"
                      });

            var target = new WirelessTagJsonClient(clientMock.Object);
            var request = new DummyRequest();

            var response = target.Login(null, null);

            Assert.NotNull(response);
            Assert.Equal("some response data", response);
        }

        [Fact]
        public void GetTagList_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTagListResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTagList();

            // assert
            Assert.False(String.IsNullOrEmpty(result));
        }

        [Fact]
        public void GetTemperatureStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "TemperatureStats2Response.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStats(1);

            // assert
            Assert.False(String.IsNullOrEmpty(result));
        }

        [Fact]
        public void GetTagSpanStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetMultiTagStatsSpanResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStats(new List<int>() { 1 });

            // assert
            Assert.False(String.IsNullOrEmpty(result));
        }

        [Fact]
        public void GetTemperatureRawData_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);

            // assert
            Assert.False(String.IsNullOrEmpty(result));
        }
    }

    internal class DummyRequest : RestRequest
    {
        public DummyRequest() : base("/myendpoint") { }
    }
}
