using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using RestSharp;
using System.Net;
using WirelessTagClientLib.Test.TestHelpers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WirelessTagClientLib.Test
{
    [TestClass]
    [DeploymentItem(@"TestData\AuthenticationFailedResponse.json")]
    [DeploymentItem(@"TestData\GetTagListResponse.json")]
    [DeploymentItem(@"TestData\GetMultiTagStatsSpanResponse.json")]
    [DeploymentItem(@"TestData\GetMultiTagStatsSpanErrorResponse.json")]
    [DeploymentItem(@"TestData\GetTemperatureRawDataResponse.json")]
    [DeploymentItem(@"TestData\GetTemperatureRawDataEmptyResponse.json")]
    [DeploymentItem(@"TestData\GetTemperatureRawDataErrorResponse.json")]
    [DeploymentItem(@"TestData\LoginErrorResponse.json")]
    [DeploymentItem(@"TestData\LoginResponse.json")]
    [DeploymentItem(@"TestData\TemperatureStats2Response.json")]
    public class WirelessTagAsyncClientTest
    {
        public TestContext TestContext { get; set; }

        #region LoginAsync
        [TestMethod]
        public void LoginAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "LoginResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.LoginAsync("user", "secret");

            // assert
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public void LoginAsync_Response_Error_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "LoginErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.LoginAsync("user", "badPassword");

            // assert
            Assert.IsFalse(result.Result);
        }
        #endregion

        #region GetTagListAsync
        [TestMethod]
        public void GetTagListAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTagListResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagListAsync();

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(2, result.Result.Count);
            AssertHelper.AssertTagInfo(result.Result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result.Result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [TestMethod]
        public void GetTagListAsync_NotLoggedIn_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "AuthenticationFailedResponse.json", HttpStatusCode.Unauthorized)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagListAsync();

            // assert
            AssertError(result);
        }
        #endregion

        #region  GetTemperatureStatsAsync
        [TestMethod]
        public void GetTemperatureStatsAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "TemperatureStats2Response.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStatsAsync(1);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(2, result.Result.HourlyReadings.Count);
            AssertHelper.AssertHourlyReading(result.Result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.Result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }

        [TestMethod]
        public void GetTemperatureStatsAsync_NotLoggedIn_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "AuthenticationFailedResponse.json", HttpStatusCode.Unauthorized)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureStatsAsync(1);

            // assert
            AssertError(result);
        }
        #endregion

        #region GetTagSpanStatsAsyn
        [TestMethod]
        public void GetTagSpanStatsAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetMultiTagStatsSpanResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStatsAsync(new List<int>() { 1 });

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(new DateTime(2020, 1, 1, 0, 0, 1), result.Result.From);
            Assert.AreEqual(new DateTime(2022, 7, 8, 0, 0, 1), result.Result.To);
            Assert.AreNotEqual(0, result.Result.TimeZoneOffset);

            // ids
            Assert.IsNotNull(result.Result.Ids);
            Assert.AreEqual(3, result.Result.Ids.Count);
            CollectionAssert.Contains(result.Result.Ids, 1);
            CollectionAssert.Contains(result.Result.Ids, 2);
            CollectionAssert.Contains(result.Result.Ids, 3);

            // names
            Assert.IsNotNull(result.Result.Names);
            Assert.AreEqual(3, result.Result.Names.Count);
            CollectionAssert.Contains(result.Result.Names, "House");
            CollectionAssert.Contains(result.Result.Names, "Garden");
            CollectionAssert.Contains(result.Result.Names, "Garage");
        }

        [TestMethod]
        public void GetTagSpanStatsAsync_TagNotFound_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetMultiTagStatsSpanErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTagSpanStatsAsync(new List<int>() { 9999 });

            // assert
            AssertError(result);
        }
        #endregion

        #region GetTemperatureRawDataAsync
        [TestMethod]
        public void GetTemperatureRawDataAsync_Response_Ok_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawDataAsync(1, DateTime.MinValue, DateTime.MinValue);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(3, result.Result.Count);
            AssertHelper.AssertTemperatureInfo(result.Result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result.Result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result.Result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }

        [TestMethod]
        public void GetTemperatureRawDataAsync_EmptyResponse_Should_Return_Valid_Response()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataEmptyResponse.json")));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawDataAsync(1, DateTime.Today, DateTime.Today.AddDays(-7)); // end date is before start date

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);

            Assert.AreEqual(0, result.Result.Count);
        }

        [TestMethod]
        public void GetTemperatureRawDataAsync_TagNotFound_Should_Throw_HttpStatusException()
        {
            // arrange
            var clientMock = new Mock<IRestClient>();
            clientMock.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>()))
                      .Returns(Task.FromResult(TestHelper.GetRestResponseFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataErrorResponse.json", HttpStatusCode.InternalServerError)));

            var target = new WirelessTagAsyncClient(clientMock.Object);

            // act
            var result = target.GetTemperatureRawDataAsync(999, DateTime.MinValue, DateTime.MinValue);

            // assert
            AssertError(result);
        }
        #endregion

        private void AssertError<T>(Task<T> task)
        {
            Assert.IsNotNull(task);
            Assert.IsTrue(task.IsFaulted);
            Assert.IsNotNull(task.Exception.InnerException);
            Assert.IsInstanceOfType(task.Exception.InnerException, typeof(HttpStatusException));
        }
    }
}
