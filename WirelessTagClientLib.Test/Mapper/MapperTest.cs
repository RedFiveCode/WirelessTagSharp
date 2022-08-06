using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using WirelessTagClientLib.RawResponses;
using WirelessTagClientLib.Test.TestHelpers;

namespace WirelessTagClientLib.Test
{
    [TestClass]
    [DeploymentItem(@"TestData\GetTagListResponse.json")]
    [DeploymentItem(@"TestData\GetMultiTagStatsSpanResponse.json")]
    [DeploymentItem(@"TestData\GetTemperatureRawDataResponse.json")]
    [DeploymentItem(@"TestData\TemperatureStats2Response.json")]
    public class MapperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Create_StatsSpanDataResponse_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(TestContext.DeploymentDirectory, "GetMultiTagStatsSpanResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<MultiTagStatsSpanResponse>(responseData);

            // act
            var result = Mapper.Create(response.Data);

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
        public void Create_TagListResponse_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(TestContext.DeploymentDirectory, "GetTagListResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TagListResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            AssertHelper.AssertTagInfo(result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [TestMethod]
        public void Create_GetTemperatureRawData_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(TestContext.DeploymentDirectory, "GetTemperatureRawDataResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TemperatureRawDataResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            AssertHelper.AssertTemperatureInfo(result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }

        [TestMethod]
        public void Create_GetTemperatureStats_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(TestContext.DeploymentDirectory, "TemperatureStats2Response.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TemperatureStatsResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.TemperatureUnits); // C

            Assert.IsNotNull(result.HourlyReadings);
            Assert.AreEqual(2, result.HourlyReadings.Count);

            AssertHelper.AssertHourlyReading(result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }
    }
}
