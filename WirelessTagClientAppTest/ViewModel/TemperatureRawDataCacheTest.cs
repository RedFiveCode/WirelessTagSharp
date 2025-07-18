using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass]
    public class TemperatureRawDataCacheTest
    {
        [TestMethod]
        public void Update_OneItem_SetsCount()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };
            var target = new TemperatureRawDataCache();

            Assert.AreEqual(0, target.Count);

            // act
            target.Update(tagId, data);

            // assert
            Assert.AreEqual(1, target.Count);
            Assert.AreEqual(2, target.ItemCount);
        }

        [TestMethod]
        public void Update_TwoItems_SetsCount()
        {
            // arrange
            const int tagId1 = 42;
            var data1 = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };

            const int tagId2 = 99;
            var data2 = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
                CreateTemperatureDataPoint(2023, 1, 2, 20)
            };

            var target = new TemperatureRawDataCache();


            Assert.AreEqual(0, target.Count);

            // act
            target.Update(tagId1, data1);
            target.Update(tagId2, data2);

            // assert
            Assert.AreEqual(2, target.Count);
            Assert.AreEqual(4, target.ItemCount);
        }

        [TestMethod]
        public void Update_DuplicateTimestamp_SetsCount()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
            };
            var dataSecond = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 25) // same time, different temperature
            };

            var target = new TemperatureRawDataCache();

            Assert.AreEqual(0, target.Count);

            target.Update(tagId, dataFirst);
            Assert.AreEqual(1, target.Count);

            // act
            target.Update(tagId, dataSecond);

            // assert
            Assert.AreEqual(1, target.ItemCount);
        }

        [TestMethod]
        public void Update_DuplicateTimestamp_StoresFirstValue()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 20),
            };
            var dataSecond = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 25) // same time, different temperature
            };

            var target = new TemperatureRawDataCache();

            Assert.AreEqual(0, target.Count);

            target.Update(tagId, dataFirst);
            Assert.AreEqual(1, target.Count);

            // act
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId);
            Assert.AreEqual(1, data.Count());

            AssertValue(data.First(), 2023, 1, 1, 20);
        }

        [TestMethod]
        public void Update_DistinctValues_AppendsValues()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11),
            };
            var dataSecond = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 2, 1, 12),
                CreateTemperatureDataPoint(2023, 2, 2, 13)
            };

            var target = new TemperatureRawDataCache();

            Assert.AreEqual(0, target.Count);

            // act
            target.Update(tagId, dataFirst);
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId).ToList();
            Assert.AreEqual(4, data.Count);

            AssertValue(data[0], 2023, 1, 1, 10);
            AssertValue(data[1], 2023, 1, 2, 11);
            AssertValue(data[2], 2023, 2, 1, 12);
            AssertValue(data[3], 2023, 2, 2, 13);
        }

        [TestMethod]
        public void Update_DistinctAndDuplicateValues_AppendsValues()
        {
            // arrange
            const int tagId = 42;
            var dataFirst = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11),
            };
            var dataSecond = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 1), // duplicate timestamp, different value than previous update
                CreateTemperatureDataPoint(2023, 1, 2, 2), // ditto
                CreateTemperatureDataPoint(2023, 2, 1, 12),
                CreateTemperatureDataPoint(2023, 2, 2, 13)
            };

            var target = new TemperatureRawDataCache();

            Assert.AreEqual(0, target.Count);

            // act
            target.Update(tagId, dataFirst);
            target.Update(tagId, dataSecond);

            // assert
            var data = target.GetData(tagId).ToList();
            Assert.AreEqual(4, data.Count);

            AssertValue(data[0], 2023, 1, 1, 10);
            AssertValue(data[1], 2023, 1, 2, 11);
            AssertValue(data[2], 2023, 2, 1, 12);
            AssertValue(data[3], 2023, 2, 2, 13);
        }

        [TestMethod]
        public void Update_MultipleTags_StoresVauesForExpectedTag()
        {
            // arrange
            const int tagId1 = 42;
            var dataFirst = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            const int tagId2 = 99;
            var dataSecond = new List<TemperatureDataPoint>()
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
            Assert.AreEqual(2, data1.Count);
            AssertValue(data1[0], 2023, 1, 1, 10);
            AssertValue(data1[1], 2023, 1, 2, 11);

            var data2 = target.GetData(tagId2).ToList();
            Assert.AreEqual(2, data2.Count);
            AssertValue(data2[0], 2023, 2, 1, 20);
            AssertValue(data2[1], 2023, 2, 2, 21);
        }

        #region ContainsDataForTag(int, DateTime) tests
        [TestMethod]
        public void ContainsDataForTag_Hit_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1));

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsDataForTag_HitSameDayDifferentTime_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                new TemperatureDataPoint(new DateTime(2023, 1, 1, 13, 0, 0), 10), // 13:00:00
                new TemperatureDataPoint(new DateTime(2023, 1, 1, 14, 0, 0), 10), // 14:00:00
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1));

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsDataForTag_MissDate_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 12, 31));

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsDataForTag_MissTagId_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(99, new DateTime(2023, 12, 31));

            // assert
            Assert.IsFalse(result);
        }
        #endregion

        #region ContainsDataForTag(int, DateTime, DateTime) tests
        [TestMethod]
        public void ContainsDataForTag3_Hit_ReturnsTrue()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                new TemperatureDataPoint(new DateTime(2023, 1, 1, 13, 0, 0), 10), // 13:00:00
                new TemperatureDataPoint(new DateTime(2023, 1, 1, 14, 0, 0), 10), // 14:00:00
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsDataForTag3_MissDate_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(tagId, new DateTime(2023, 12, 1), new DateTime(2023, 12, 31));

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsDataForTag3_MissTagId_ReturnsFalse()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act
            var result = target.ContainsDataForTag(99, new DateTime(2023, 1, 11), new DateTime(2023, 12, 31));

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ContainsDataForTag3_EndBeforeStart_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            const int tagId = 42;
            var data = new List<TemperatureDataPoint>()
            {
                CreateTemperatureDataPoint(2023, 1, 1, 10),
                CreateTemperatureDataPoint(2023, 1, 2, 11)
            };

            var target = new TemperatureRawDataCache();
            target.Update(tagId, data);

            // act; should throw
            var result = target.ContainsDataForTag(42, new DateTime(2025, 1, 1), new DateTime(2023, 1, 1));
        }
        #endregion

        private TemperatureDataPoint CreateTemperatureDataPoint(int year, int month, int day, double temperature)
        {
            return new TemperatureDataPoint(new DateTime(year, month, day), temperature);
        }

        private void AssertValue(TemperatureDataPoint item, int year, int month, int day, double temperature)
        {
            var dt = new DateTime(year, month, day);

            Assert.AreEqual(dt, item.Time);
            Assert.AreEqual(temperature, item.Temperature);
        }

    }
}
