using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Utils
{
    /// <summary>
    /// Unit tests for teh <see cref="MinMaxMeasurementViewModelSerializer"/> class.
    /// </summary>
    [TestClass]
    public class MinMaxMeasurementViewModelSerializerTest
    {
        [ExpectedException(typeof(ArgumentNullException))]  
        [TestMethod]
        public void SerialiseToJson_Null_ShouldThrowArgumentNullException()
        {
            // act
            MinMaxMeasurementViewModelSerializer.SerialiseToJson(null); 
        }

        [TestMethod]
        public void SerialiseToJson_ShouldReturnJsonString()
        {
            // arrange
            var data = CreateViewModel();

            // act
            var json = MinMaxMeasurementViewModelSerializer.SerialiseToJson(data);

            // assert
            Assert.IsNotNull(json);
            Assert.IsTrue(json.Length > 0);

            var expected = @"{""TagId"":1,""TagName"":""my tag"",""Interval"":0,""IntervalFrom"":""2022-12-17T00:00:00"",""IntervalTo"":""2022-12-17T23:59:59"",""Minimum"":{""Timestamp"":""2022-12-17T04:00:00"",""Temperature"":-2.0,""IsToday"":true},""Maximum"":{""Timestamp"":""2022-12-17T14:00:00"",""Temperature"":5.0,""IsToday"":true},""Count"":10,""Difference"":7.0}";
            Assert.AreEqual(expected, json);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void DeserialiseToJson_Null_ShouldThrowArgumentNullException()
        {
            // act
            MinMaxMeasurementViewModelSerializer.Deserialise(null);
        }

        [TestMethod]
        public void SerialiseDeserialiseRoundTrip_ShouldEquivalentObject()
        {
            // arrange
            var original = CreateViewModel();

            // act
            var json = MinMaxMeasurementViewModelSerializer.SerialiseToJson(original);

            var clone = MinMaxMeasurementViewModelSerializer.Deserialise(json);

            // assert
            Assert.IsNotNull(clone);

            Assert.AreEqual(original.Count, clone.Count);
            Assert.AreEqual(original.TagId, clone.TagId);
            Assert.AreEqual(original.TagName, clone.TagName);
            Assert.AreEqual(original.Interval, clone.Interval);

            Assert.AreEqual(original.IntervalFrom, clone.IntervalFrom);
            Assert.AreEqual(original.IntervalTo, clone.IntervalTo);

            Assert.AreEqual(original.Minimum.Temperature, clone.Minimum.Temperature);
            Assert.AreEqual(original.Minimum.Timestamp, clone.Minimum.Timestamp);

            Assert.AreEqual(original.Maximum.Temperature, clone.Maximum.Temperature);
            Assert.AreEqual(original.Maximum.Timestamp, clone.Maximum.Timestamp);
        }

        private MinMaxMeasurementViewModel CreateViewModel()
        {
            var data = new MinMaxMeasurementViewModel()
            {
                Interval = TimeInterval.Today,
                Count = 10,
                IntervalFrom = new DateTime(2022, 12, 17),
                IntervalTo = new DateTime(2022, 12, 17, 23, 59, 59),
                TagId = 1,
                TagName = "my tag",
                Minimum = new Measurement() { Temperature = -2d, Timestamp = new DateTime(2022, 12, 17, 4, 0, 0) },
                Maximum = new Measurement() { Temperature = 5d, Timestamp = new DateTime(2022, 12, 17, 14, 0, 0) }
            };

            return data;
        }
    }
}
