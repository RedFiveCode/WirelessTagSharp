using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using WirelessTagClientLib.Test.TestHelpers;

namespace WirelessTagClientLib.Test
{
    [TestClass]
    [DeploymentItem(@"TestData\GetTagListResponse.json")]
    [DeploymentItem(@"TestData\GetMultiTagStatsSpanResponse.json")]
    [DeploymentItem(@"TestData\GetTemperatureRawDataResponse.json")]
    [DeploymentItem(@"TestData\LoginErrorResponse.json")]
    [DeploymentItem(@"TestData\LoginResponse.json")]
    [DeploymentItem(@"TestData\TemperatureStats2Response.json")]
    public class WirelessTagJsonClientTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Execute_Response_Not_Ok_Should_Throw_HttpStatusException()
        {
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "LoginErrorResponse.json", HttpStatusCode.InternalServerError));

            var target = new WirelessTagJsonClient(clientMock.Object);
            var request = new DummyRequest();

            var response = target.Login(null, null);
        }

        [TestMethod]
        public void Execute_Response_Ok_Should_Return_Valid_Response()
        {
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(new RestResponse()
                       {  StatusCode = HttpStatusCode.OK,
                          Content = "some response data",
                          ContentType = "text/plain" });

            var target = new WirelessTagJsonClient(clientMock.Object);
            var request = new DummyRequest();

            var response = target.Login(null, null);

            Assert.IsNotNull(response);
            Assert.AreEqual("some response data", response);
        }

        [TestMethod]
        public void GetTagList_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTagListResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTagList();

            // assert
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GetTemperatureStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "TemperatureStats2Response.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStats(1);

            // assert
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GetTagSpanStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetMultiTagStatsSpanResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStats(new List<int>() { 1 });

            // assert
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void GetTemperatureRawData_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataResponse.json"));

            var target = new WirelessTagJsonClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);

            // assert
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }
    }

    internal class DummyRequest : RestRequest
    {
        public DummyRequest() : base("/myendpoint") { }
    }
}
