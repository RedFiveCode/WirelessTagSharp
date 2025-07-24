using Newtonsoft.Json;
using System;
using WirelessTagClientLib.RawResponses;
using WirelessTagClientLib.Test.TestHelpers;
using Xunit;

namespace WirelessTagClientLib.Test
{
    public class MapperTest
    {
        [Fact]
        public void Create_StatsSpanDataResponse_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(AppContext.BaseDirectory, "GetMultiTagStatsSpanResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<MultiTagStatsSpanResponse>(responseData);

            // act
            var result = Mapper.Create(response.Data);

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
        public void Create_TagListResponse_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(AppContext.BaseDirectory, "GetTagListResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TagListResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            AssertHelper.AssertTagInfo(result[0], "AAAA", "Tag 1", Guid.Parse("11111111-2222-3333-4444-555555555555"));
            AssertHelper.AssertTagInfo(result[1], "BBBB", "Tag 2", Guid.Parse("22222222-2222-3333-4444-555555555555"));
        }

        [Fact]
        public void Create_GetTemperatureRawData_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(AppContext.BaseDirectory, "GetTemperatureRawDataResponse.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TemperatureRawDataResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            AssertHelper.AssertTemperatureInfo(result[0], new DateTime(2022, 7, 9, 0, 16, 7), 16.9);
            AssertHelper.AssertTemperatureInfo(result[1], new DateTime(2022, 7, 9, 14, 30, 36), 19.89);
            AssertHelper.AssertTemperatureInfo(result[2], new DateTime(2022, 7, 9, 15, 3, 10), 20.3);
        }

        [Fact]
        public void Create_GetTemperatureStats_Should_Return_ValidObject()
        {
            // arrange
            var responseData = TestHelper.GetResponseDataFromFile(AppContext.BaseDirectory, "TemperatureStats2Response.json"); // text file containing json response
            var response = JsonConvert.DeserializeObject<TemperatureStatsResponse>(responseData);

            // act
            var result = Mapper.Create(response);

            // assert
            Assert.NotNull(result);

            Assert.Equal(0, result.TemperatureUnits); // C

            Assert.NotNull(result.HourlyReadings);
            Assert.Equal(2, result.HourlyReadings.Count);

            AssertHelper.AssertHourlyReading(result.HourlyReadings[0], new DateTime(2022, 7, 8));
            AssertHelper.AssertHourlyReading(result.HourlyReadings[1], new DateTime(2022, 7, 9));
        }
    }
}
