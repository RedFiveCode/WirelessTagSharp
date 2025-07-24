using Moq;
using System;
using RestSharp;
using System.Net;
using System.Collections.Generic;
using WirelessTagClientLib.Test.TestHelpers;
using Xunit;

namespace WirelessTagClientLib.Test
{
    public class WirelessTagClientTest
    {
        [Fact]
        public void Ctor_Sets_Url_Property()
        {
            var target = new WirelessTagClient(string.Empty);

            Assert.Equal(WirelessTagConstants.Url, target.Url);
        }

        [Fact]
        public void Ctor_IRestClient_Sets_Url_Property()
        {
            var clientMock = new Mock<IRestClient>();

            var target = new WirelessTagClient(clientMock.Object);

            Assert.Equal(WirelessTagConstants.Url, target.Url);
        }

        [Fact]
        public void Execute_Response_Not_Ok_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(new RestResponse() { StatusCode = HttpStatusCode.BadRequest });

            // act - should throw
            var target = new WirelessTagClient(clientMock.Object);

            // assert
            Assert.Throws<HttpStatusException>(() => target.GetTagList());
        }

        [Fact]
        public void Login_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "LoginResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.Login("user", "secret");

            // assert
            Assert.True(result);
        }

        [Fact]
        public void Login_Response_Error_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Throws(new HttpStatusException(HttpStatusCode.InternalServerError, "Internal Server Error"));

            var target = new WirelessTagClient(clientMock.Object);

            // act - should throw
            Assert.Throws<HttpStatusException>(() => target.Login("user", "secret"));
        }

        [Fact]
        public void GetTagList_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTagListResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTagList();

            // assert
            Assert.NotNull(result);

            Assert.Equal(2, result.Count);
            AssertHelper.AssertTagInfo(result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [Fact]
        public void GetTemperatureStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "TemperatureStats2Response.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStats(1);

            // assert
            Assert.NotNull(result.HourlyReadings);

            Assert.Equal(2, result.HourlyReadings.Count);
            AssertHelper.AssertHourlyReading(result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }

        [Fact]
        public void GetTagSpanStats_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetMultiTagStatsSpanResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStats(new List<int>() { 1 });

            // assert
            Assert.NotNull(result);

            Assert.NotEqual(DateTime.MinValue, result.From);
            Assert.NotEqual(DateTime.MinValue, result.To);
            Assert.NotEqual(0, result.TimeZoneOffset);

            // ids
            Assert.NotNull(result.Ids);
            Assert.Equal(3, result.Ids.Count);
            Assert.Contains(1, result.Ids);
            Assert.Contains(2, result.Ids);
            Assert.Contains(3, result.Ids);

            // names
            Assert.NotNull(result.Names);
            Assert.Equal(3, result.Names.Count);
            Assert.Contains("House", result.Names);
            Assert.Contains("Garden", result.Names);
            Assert.Contains("Garage", result.Names);
        }

        [Fact]
        public void GetTemperatureRawData_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.Execute(It.IsAny<RestRequest>()))
                      .Returns(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataResponse.json"));

            var target = new WirelessTagClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawData(1, DateTime.Today, DateTime.Today);

            // assert
            Assert.NotNull(result);

            Assert.Equal(3, result.Count);
            AssertHelper.AssertTemperatureInfo(result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }
    }
}
