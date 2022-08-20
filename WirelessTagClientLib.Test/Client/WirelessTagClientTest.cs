using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using RestSharp;
using System.Net;
using System.Collections.Generic;
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
    public class WirelessTagClientTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Ctor_Sets_Url_Property()
        {
            var target = new WirelessTagClient();

            Assert.AreEqual(WirelessTagConstants.Url, target.Url);
        }

        [TestMethod]
        public void Ctor_IRestClient_Sets_Url_Property()
        {
            var clientMock = new Mock<IRestClient>();

            var target = new WirelessTagClient(clientMock.Object);

            Assert.AreEqual(WirelessTagConstants.Url, target.Url);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpStatusException))]
        public void Execute_Response_Not_Ok_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(new RestResponse() { StatusCode = HttpStatusCode.BadRequest });

            // act - should throw
            var target = new WirelessTagClient(clientMock.Object);

            // assert
            target.GetTagList();
        }

        [TestMethod]
        public void Login_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "LoginResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.Login("user", "secret");

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Login_Response_Error_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "LoginErrorResponse.json", HttpStatusCode.InternalServerError));

            var target = new WirelessTagClient(clientMock.Object);

            // act - should throw
            var result = target.Login("user", "secret");
        }

        [TestMethod]
        public void GetTagList_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTagListResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTagList();

            // assert
            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);
            AssertHelper.AssertTagInfo(result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [TestMethod]
        public void GetTemperatureStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "TemperatureStats2Response.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStats(1);

            // assert
            Assert.IsNotNull(result.HourlyReadings);

            Assert.AreEqual(2, result.HourlyReadings.Count);
            AssertHelper.AssertHourlyReading(result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }

        [TestMethod]
        public void GetTagSpanStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetMultiTagStatsSpanResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStats(new List<int>() { 1 });

            // assert
            Assert.IsNotNull(result);

            Assert.AreNotEqual(DateTime.MinValue, result.From);
            Assert.AreNotEqual(DateTime.MinValue, result.To);
            Assert.AreNotEqual(0, result.TimeZoneOffset);

            // ids
            Assert.IsNotNull(result.Ids);
            Assert.AreEqual(3, result.Ids.Count);
            CollectionAssert.Contains(result.Ids, 1);
            CollectionAssert.Contains(result.Ids, 2);
            CollectionAssert.Contains(result.Ids, 3);

            // names
            Assert.IsNotNull(result.Names);
            Assert.AreEqual(3, result.Names.Count);
            CollectionAssert.Contains(result.Names, "House");
            CollectionAssert.Contains(result.Names, "Garden");
            CollectionAssert.Contains(result.Names, "Garage");
        }

        [TestMethod]
        public void GetTemperatureRawData_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);

            // assert
            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Count);
            AssertHelper.AssertTemperatureInfo(result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }
    }
}
