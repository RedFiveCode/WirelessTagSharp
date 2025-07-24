using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.ViewModel
{
    /// <summary>
    /// Unit tests for the TemperatureRawDataCache class
    /// </summary>
    
    public class TemperatureRawDataCacheTest
    {
        [Fact]
        public void Update_OneItem_SetsCount()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };
            var target = new TemperatureRawDataCache();

            Assert.Equal(0, target.Count);

            // act
            target.Update(tagId, data);

            // assert
            Assert.Equal(1, target.Count);
            Assert.Equal(2, target.ItemCount);
        }

        [Fact]
        public void Update_TwoItems_SetsCount()
        {
            // arrange
            const int tagId1 = 42;
            var data1 = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };

            const int tagId2 = 99;
            var data2 = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };

            var target = new TemperatureRawDataCache();


            Assert.Equal(0, target.Count);

            // act
            target.Update(tagId1, data1);
            target.Update(tagId2, data2);

            // assert
            Assert.Equal(2, target.Count);
            Assert.Equal(4, target.ItemCount);
        }

        [Fact]
        public void Update_DuplicateTimestamp_SetsCount()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
            };
            var dataSecond = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 25) // same time, different temperature
            };

            var target = new TemperatureRawDataCache();

            Assert.Equal(0, target.Count);

            target.Update(tagId, dataFirst);
            Assert.Equal(1, target.Count);

            // act
            target.Update(tagId, dataSecond);

            // assert
            Assert.Equal(1, target.ItemCount);
        }

        [Fact]
        public void Update_DuplicateTimestamp_StoresFirstValue()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
            };
            var dataSecond = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 25) // same time, different temperature
            };

            var target = new TemperatureRawDataCache();

            Assert.Equal(0, target.Count);

            target.Update(tagId, dataFirst);
            Assert.Equal(1, target.Count);

            // act
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId);
            Assert.Equal(1, data.Count());

            AssertValue(data.First(), 2023, 1, 1, 20);
        }

        [Fact]
        public void Update_DistinctValues_AppendsValues()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11),
            };
            var dataSecond = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 2, 1, 12),
                CreateTemperatureDataPoint(2023, 2, 2, 13)
            };

            var target = new TemperatureRawDataCache();

            Assert.Equal(0, target.Count);

            // act
            target.Update(tagId, dataFirst);
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId).ToList();
            Assert.Equal(4, data.Count);

            AssertValue(data[0], 2023, 1, 1, 10);
            AssertValue(data[1], 2023, 1, 2, 11);
            AssertValue(data[2], 2023, 2, 1, 12);
            AssertValue(data[3], 2023, 2, 2, 13);
        }

        [Fact]
        public void Update_DistinctAndDuplicateValues_AppendsValues()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11),
            };
            var dataSecond = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 1), // duplicate timestamp, different value than previous update
                CreateTemperatureDataPoint(2023, 1, 2, 2), // ditto
                CreateTemperatureDataPoint(2023, 2, 1, 12),
                CreateTemperatureDataPoint(2023, 2, 2, 13)
            };

            var target = new TemperatureRawDataCache();

            Assert.Equal(0, target.Count);

            // act
            target.Update(tagId, dataFirst);
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId).ToList();
            Assert.Equal(4, data.Count);

            AssertValue(data[0], 2023, 1, 1, 10);
            AssertValue(data[1], 2023, 1, 2, 11);
            AssertValue(data[2], 2023, 2, 1, 12);
            AssertValue(data[3], 2023, 2, 2, 13);
        }

        [Fact]
        public void Update_MultipleTags_StoresVauesForExpectedTag()
        {
            // arrange
            const int tagId1 = 42;
            var dataFirst = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            const int tagId2 = 99;
            var dataSecond = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 2, 1, 20),
                CreateTemperatureDataPoint(2023, 2, 2, 21)
            };

            var target = new TemperatureRawDataCache();

            // act
            target.Update(tagId1, dataFirst);
            target.Update(tagId2, dataSecond);

            // assert
            var data1 = target.GetData(tagId1).ToList();
            Assert.Equal(2, data1.Count);
            AssertValue(data1[0], 2023, 1, 1, 10);
            AssertValue(data1[1], 2023, 1, 2, 11);

            var data2 = target.GetData(tagId2).ToList();
            Assert.Equal(2, data2.Count);
            AssertValue(data2[0], 2023, 2, 1, 20);
            AssertValue(data2[1], 2023, 2, 2, 21);
        }

        #region ContainsDataForTag(int, DateTime) tests
        [Fact]
        public void ContainsDataForTag_Hit_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1));

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ContainsDataForTag_HitSameDayDifferentTime_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                new Measurement(new DateTime(2023, 1, 1, 13, 0, 0), 10), // 13:00:00
                new Measurement(new DateTime(2023, 1, 1, 14, 0, 0), 10), // 14:00:00
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1));

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ContainsDataForTag_MissDate_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 12, 31));

            // assert
            Assert.False(result);
        }

        [Fact]
        public void ContainsDataForTag_MissTagId_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(99, new DateTime(2023, 12, 31));

            // assert
            Assert.False(result);
        }
        #endregion

        #region ContainsDataForTag(int, DateTime, DateTime) tests
        [Fact]
        public void ContainsDataForTag3_Hit_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                new Measurement(new DateTime(2023, 1, 1, 13, 0, 0), 10), // 13:00:00
                new Measurement(new DateTime(2023, 1, 1, 14, 0, 0), 10), // 14:00:00
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));

            // assert
            Assert.True(result);
        }

        [Fact]
        public void ContainsDataForTag3_MissDate_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 12, 1), new DateTime(2023, 12, 31));

            // assert
            Assert.False(result);
        }

        [Fact]
        public void ContainsDataForTag3_MissTagId_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(99, new DateTime(2023, 1, 11), new DateTime(2023, 12, 31));

            // assert
            Assert.False(result);
        }

        [Fact]
        public void ContainsDataForTag3_EndBeforeStart_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            const int tagId = 42;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act; should throw
            Assert.Throws<ArgumentOutOfRangeException>(() => target.ContainsDataForTag(42, new DateTime(2025, 1, 1), new DateTime(2023, 1, 1)));
        }
        #endregion

        [Fact]
        public void GetAllData_NoData_ReturnsEmpty()
        {
            // arrange
            var target = new TemperatureRawDataCache();

            // act
            var allData = target.GetAllData();

            // assert
           Assert.NotNull(allData);
            Assert.Equal(0, allData.Count());
        }

        [Fact]
        public void GetAllData_OneTag_ReturnsAllDataWithCorrectTagId()
        {
            // arrange
            var target = new TemperatureRawDataCache();
            const int tagId = 1;
            var data = new List<Measurement>
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };
            target.Update(tagId, data);

            // act
            var allData = target.GetAllData().ToList();

            // assert
            Assert.Equal(2, allData.Count);
            AssertTagMeasurementValue(allData[0], tagId, 2023, 1, 1, 10);
            AssertTagMeasurementValue(allData[1], tagId, 2023, 1, 2, 11);
        }

        [Fact]
        public void GetAllData_MultipleTags_ReturnsAllDataWithCorrectTagIds()
        {
            // arrange
            var target = new TemperatureRawDataCache();
            const int tagId1 = 1;
            const int tagId2 = 2;
            var data1 = new List<Measurement>
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };
            var data2 = new List<Measurement>
            {
                CreateTemperatureDataPoint(2023, 2, 1, 20),
                CreateTemperatureDataPoint(2023, 2, 2, 21)
            };
            target.Update(tagId1, data1);
            target.Update(tagId2, data2);

            // act
            var allData = target.GetAllData().ToList();

            // assert
            Assert.Equal(4, allData.Count);
            // The order is not guaranteed, so check that all expected values are present
            Assert.True(allData.Any(d => d.TagId == tagId1 && d.Time == new DateTime(2023, 1, 1) && d.Temperature == 10));
            Assert.True(allData.Any(d => d.TagId == tagId1 && d.Time == new DateTime(2023, 1, 2) && d.Temperature == 11));
            Assert.True(allData.Any(d => d.TagId == tagId2 && d.Time == new DateTime(2023, 2, 1) && d.Temperature == 20));
            Assert.True(allData.Any(d => d.TagId == tagId2 && d.Time == new DateTime(2023, 2, 2) && d.Temperature == 21));
        }

        private Measurement CreateTemperatureDataPoint(int year, int month, int day, double temperature)
        {
            return new Measurement(new DateTime(year, month, day), temperature);
        }

        private void AssertValue(Measurement item, int year, int month, int day, double temperature)
        {
            var dt = new DateTime(year, month, day);

            Assert.Equal(dt, item.Time);
            Assert.Equal(temperature, item.Temperature);
        }

        private void AssertTagMeasurementValue(TagMeasurementDataPoint item, int tagId, int year, int month, int day, double temperature)
        {
            var dt = new DateTime(year, month, day);
            Assert.Equal(tagId, item.TagId);
            Assert.Equal(dt, item.Time);
            Assert.Equal(temperature, item.Temperature);
        }

    }
}
