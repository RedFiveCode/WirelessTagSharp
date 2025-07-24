using Moq;
using System;
using System.Linq;
using RestSharp;
using System.Net;
using WirelessTagClientLib.Test.TestHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace WirelessTagClientLib.Test
{
    public class WirelessTagAsyncClientTest
    {
        [Fact]
        public void Ctor_Sets_Url_Property()
        {
            var target = new WirelessTagAsyncClient(string.Empty);

            Assert.Equal(WirelessTagConstants.Url, target.Url);
        }

        [Fact]
        public void Ctor_IRestClient_Sets_Url_Property()
        {
            var clientMock = new Mock<IRestClient>();

            var target = new WirelessTagAsyncClient(clientMock.Object);

            Assert.Equal(WirelessTagConstants.Url, target.Url);
        }

        #region LoginAsync
        [Fact]
        public async Task LoginAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "LoginResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.LoginAsync("user", "secret");

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task LoginAsync_Response_Error_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "LoginErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.LoginAsync("user", "badPassword");

            // assert
            Assert.False(result);
        }
        #endregion

        #region GetTagListAsync
        [Fact]
        public async Task GetTagListAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTagListResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.GetTagListAsync();

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result);

            Assert.Equal(2, result.Count);
            AssertHelper.AssertTagInfo(result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [Fact]
        public void GetTagListAsync_NotLoggedIn_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "AuthenticationFailedResponse.json", HttpStatusCode.Unauthorized)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagListAsync();

            // assert
            AssertError(result);
        }
        #endregion

        #region  GetTemperatureStatsAsync
        [Fact]
        public async Task GetTemperatureStatsAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "TemperatureStats2Response.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.GetTemperatureStatsAsync(1);

            // assert
            Assert.NotNull(result);

            Assert.Equal(2, result.HourlyReadings.Count);
            AssertHelper.AssertHourlyReading(result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }

        [Fact]
        public void GetTemperatureStatsAsync_NotLoggedIn_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "AuthenticationFailedResponse.json", HttpStatusCode.Unauthorized)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStatsAsync(1);

            // assert
            AssertError(result);
        }
        #endregion

        #region GetTagSpanStatsAsync
        [Fact]
        public async Task GetTagSpanStatsAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetMultiTagStatsSpanResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.GetTagSpanStatsAsync(new List<int>() { 1 });

            // assert
            Assert.NotNull(result);

            Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 1), result.From);
            Assert.Equal(new DateTime(2022, 7, 8, 0, 0, 1), result.To);
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
        public void GetTagSpanStatsAsync_TagNotFound_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetMultiTagStatsSpanErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStatsAsync(new List<int>() { 9999 });

            // assert
            AssertError(result);
        }
        #endregion

        #region GetTemperatureRawDataAsync
        [Fact]
        public async Task GetTemperatureRawDataAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.GetTemperatureRawDataAsync(1, DateTime.MinValue, DateTime.MinValue);

            // assert
            Assert.NotNull(result);

            Assert.Equal(3, result.Count);
            AssertHelper.AssertTemperatureInfo(result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }

        [Fact]
        public async Task GetTemperatureRawDataAsync_EmptyResponse_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataEmptyResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = await target.GetTemperatureRawDataAsync(1, DateTime.Today, DateTime.Today.AddDays(-7)); // end date is before start date

            // assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetTemperatureRawDataAsync_TagNotFound_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawDataAsync(999, DateTime.MinValue, DateTime.MinValue);

            // assert
            AssertError(result);
        }
        #endregion

        private void AssertError<T>(Task<T> task)
        {
            Assert.NotNull(task);

            Assert.True(task.IsFaulted);
            Assert.NotNull(task.Exception.InnerException);
            Assert.IsAssignableFrom<HttpStatusException>(task.Exception.InnerException);
        }
    }
}
