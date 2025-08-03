using Xunit;
using System;
using System.Collections.Generic;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.Common
{
    public class ViewModelFactoryTest
    {
        [Fact]
        public void CreateTagViewModel_Should_Return_ValidObject()
        {
            var dto = CreateTagInfo();

            var result = ViewModelFactory.CreateTagViewModel(dto);

            Assert.Equal(1, result.Id);
            Assert.Equal("My tag name", result.Name);
            Assert.Equal("Description text", result.Description);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), result.Uuid);
            Assert.Equal(21.5, result.Temperature);
            Assert.Equal(50, result.RelativeHumidity);
            Assert.Equal(1.5, result.BatteryVoltage);
            Assert.Equal(75, result.BatteryRemaining);
            Assert.Equal(42, result.SignalStrength);
            Assert.Equal(new DateTime(2022, 1, 1), result.LastCommunication);
            Assert.False(result.IsHumidityTag);
        }


        [Fact]
        public void CreateTagViewModelList_Empty_Should_Return_ValidObject()
        {
            List<TagInfo> tags = null;

            var result = ViewModelFactory.CreateTagViewModelList(tags);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateTagViewModelList_Should_Return_ValidObject()
        {
            List<TagInfo> tags = new List<TagInfo>()
            {
                CreateTagInfo()
            };

            var result = ViewModelFactory.CreateTagViewModelList(tags);

            Assert.NotNull(result);
            Assert.Single(result);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("My tag name", result[0].Name);
            Assert.Equal("Description text", result[0].Description);
            Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), result[0].Uuid);
            Assert.Equal(21.5, result[0].Temperature);
            Assert.Equal(50, result[0].RelativeHumidity);
            Assert.Equal(1.5, result[0].BatteryVoltage);
            Assert.Equal(75, result[0].BatteryRemaining); // should multiply by 100 so scaled 0 to 100
            Assert.Equal(42, result[0].SignalStrength);
            Assert.Equal(new DateTime(2022, 1, 1), result[0].LastCommunication);
            Assert.Equal(TagViewModel.ViewMode.Temperature, result[0].Mode);
        }

        [Fact]
        public void CreateTagViewModelList_Should_Return_ValidObject_WithExistingMode()
        {
            List<TagInfo> tags = new List<TagInfo>()
            {
                CreateTagInfo()
            };

            var result = ViewModelFactory.CreateTagViewModelList(tags, TagViewModel.ViewMode.VerboseDetails);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(TagViewModel.ViewMode.VerboseDetails, result[0].Mode);
        }

        [Fact]
        public void CreateRowViewModel_DataNull_Should_Return_Null()
        {
            var tag = CreateTagInfo();

            var result = ViewModelFactory.CreateRowViewModel(null, tag, TimeInterval.Today);

            Assert.Null(result);
        }

        [Fact]
        public void CreateRowViewModel_DataEmpty_Should_Return_Null()
        {
            var tag = CreateTagInfo();
            var data = new List<Measurement>(); // empty list

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Today);

            Assert.Null(result);
        }

        [Fact]
        public void CreateRowViewModel_NoDataWithinTimeInterval_Should_Return_Null()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;

            var data = new List<Measurement>()
            {
                // only data for today
                CreateTemperatureDataPoint(today.AddHours(10), 10d),
                CreateTemperatureDataPoint(today.AddHours(11), 9d),
                CreateTemperatureDataPoint(today.AddHours(12), 11d),
                CreateTemperatureDataPoint(today.AddHours(13), 15d),
            };

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Yesterday);

            Assert.Null(result);
        }

        [Fact]
        public void CreateRowViewModel_Today_Should_Return_ValidObject()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;
            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(today.AddHours(10), 10d),
                CreateTemperatureDataPoint(today.AddHours(11), 9d), // lowest temperature
                CreateTemperatureDataPoint(today.AddHours(12), 11d),
                CreateTemperatureDataPoint(today.AddHours(13), 15d), // highest temperature
            };

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Today);

           Assert.NotNull(result);
            Assert.Equal(tag.SlaveId, result.TagId); // 1
            Assert.Equal(tag.Name, result.TagName); // My tag name

            Assert.Equal(TimeInterval.Today, result.Interval);
            Assert.Equal(today, result.IntervalFrom);
            Assert.Equal(today.AddHours(23).AddMinutes(59).AddSeconds(59), result.IntervalTo);
            
            Assert.Equal(9d, result.Minimum.Temperature);
            Assert.Equal(today.AddHours(11), result.Minimum.Timestamp); // 11:00:00

            Assert.Equal(15d, result.Maximum.Temperature);
            Assert.Equal(today.AddHours(13), result.Maximum.Timestamp); // 13:00:00

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void CreateRowViewModel_Yesterday_Should_Return_ValidObject()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;
            var yesterday = today.AddDays(-1);

            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(yesterday.AddHours(0), 10d),
                CreateTemperatureDataPoint(yesterday.AddHours(2), 9d), // lowest temperature
                CreateTemperatureDataPoint(yesterday.AddHours(4), 11d),
                CreateTemperatureDataPoint(yesterday.AddHours(6), 15d), // highest temperature

                CreateTemperatureDataPoint(today.AddHours(10), 10d),
                CreateTemperatureDataPoint(today.AddHours(11), 9d),
                CreateTemperatureDataPoint(today.AddHours(12), 11d),
                CreateTemperatureDataPoint(today.AddHours(13), 15d),
            };

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Yesterday);

           Assert.NotNull(result);
            Assert.Equal(tag.SlaveId, result.TagId); // 1
            Assert.Equal(tag.Name, result.TagName); // My tag name

            Assert.Equal(TimeInterval.Yesterday, result.Interval);
            Assert.Equal(yesterday, result.IntervalFrom);
            Assert.Equal(yesterday.AddHours(23).AddMinutes(59).AddSeconds(59), result.IntervalTo);

            Assert.Equal(9d, result.Minimum.Temperature);
            Assert.Equal(yesterday.AddHours(2), result.Minimum.Timestamp); // 02:00:00

            Assert.Equal(15d, result.Maximum.Temperature);
            Assert.Equal(yesterday.AddHours(6), result.Maximum.Timestamp); // 04:00:00

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void CreateRowViewModel_Last7Days_Should_Return_ValidObject()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;

            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(today.AddDays(-8).AddHours(1), 1d), // lowest temperature, but beyond 7 days ago
                CreateTemperatureDataPoint(today.AddDays(-8).AddHours(2), 25d), // highest temperature, but beyond 7 days ago

                CreateTemperatureDataPoint(today.AddDays(-7).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-7).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-6).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-6).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-5).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-5).AddHours(2), 20d), // highest temperature within 7 day range

                CreateTemperatureDataPoint(today.AddDays(-4).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-4).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-3).AddHours(1), 2d), // lowest temperature within 7 day range
                CreateTemperatureDataPoint(today.AddDays(-3).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-2).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-2).AddHours(2), 11d),

                // yesterday
                CreateTemperatureDataPoint(today.AddDays(-1).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-1).AddHours(2), 11d),

                // today
                CreateTemperatureDataPoint(today.AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddHours(2), 9d),
            };

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Last7Days);

           Assert.NotNull(result);
            Assert.Equal(tag.SlaveId, result.TagId); // 1
            Assert.Equal(tag.Name, result.TagName); // My tag name

            Assert.Equal(TimeInterval.Last7Days, result.Interval);
            Assert.Equal(today.AddDays(-7), result.IntervalFrom);
            Assert.Equal(today.AddHours(23).AddMinutes(59).AddSeconds(59), result.IntervalTo);

            Assert.Equal(2d, result.Minimum.Temperature);
            Assert.Equal(today.AddDays(-3).AddHours(1), result.Minimum.Timestamp);

            Assert.Equal(20d, result.Maximum.Temperature);
            Assert.Equal(today.AddDays(-5).AddHours(2), result.Maximum.Timestamp);

            Assert.Equal(16, result.Count); // 16 of the 18 data points are in teh 7 day time span
        }

        [Fact]
        public void CreateRowViewModel_Last30Days_Should_Return_ValidObject()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;

            var data = new List<Measurement>()
            {
                CreateTemperatureDataPoint(today.AddDays(-31).AddHours(1), 1d), // lowest temperature, but beyond 30 days ago
                CreateTemperatureDataPoint(today.AddDays(31).AddHours(2), 25d), // highest temperature, but beyond 30 days ago

                CreateTemperatureDataPoint(today.AddDays(-30).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-30).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-25).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-25).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-15).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-15).AddHours(2), 20d), // highest temperature within 30 day range

                CreateTemperatureDataPoint(today.AddDays(-10).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-10).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-5).AddHours(1), 2d), // lowest temperature within 30 day range
                CreateTemperatureDataPoint(today.AddDays(-5).AddHours(2), 11d),

                CreateTemperatureDataPoint(today.AddDays(-2).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-2).AddHours(2), 11d),

                // yesterday
                CreateTemperatureDataPoint(today.AddDays(-1).AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddDays(-1).AddHours(2), 11d),

                // today
                CreateTemperatureDataPoint(today.AddHours(1), 10d),
                CreateTemperatureDataPoint(today.AddHours(2), 9d),
            };

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.Last30Days);

           Assert.NotNull(result);
            Assert.Equal(tag.SlaveId, result.TagId); // 1
            Assert.Equal(tag.Name, result.TagName); // My tag name

            Assert.Equal(TimeInterval.Last30Days, result.Interval);
            Assert.Equal(today.AddDays(-30), result.IntervalFrom);
            Assert.Equal(today.AddHours(23).AddMinutes(59).AddSeconds(59), result.IntervalTo);

            Assert.Equal(2d, result.Minimum.Temperature);
            Assert.Equal(today.AddDays(-5).AddHours(1), result.Minimum.Timestamp);

            Assert.Equal(20d, result.Maximum.Temperature);
            Assert.Equal(today.AddDays(-15).AddHours(2), result.Maximum.Timestamp);
        }

        [Fact]
        public void CreateRowViewModel_ThisYear_Should_Return_ValidObject()
        {
            var tag = CreateTagInfo();

            var today = DateTime.Now.Date;
            var startOfYear = new DateTime(today.Year, 1, 1);

            var dayOfYear = today.DayOfYear;

            // Data so far this year...
            // if today is early in the year, say 2nd January, then not much data to consider
            // so need to be careful in constructing test data to ensure that it has not fallen before the start of the current year



            var data = new List<Measurement>();


            DateTime dt = today;
            while (dt >= startOfYear)
            {
                data.Add(CreateTemperatureDataPoint(dt, 10d));

                dt = dt.AddDays(-1);
            }

            // the min and max temperatures over the year to date occur today
            data.Add(CreateTemperatureDataPoint(today.AddHours(1), 0d));
            data.Add(CreateTemperatureDataPoint(today.AddHours(2), 100d));

            var result = ViewModelFactory.CreateRowViewModel(data, tag, TimeInterval.ThisYear);

           Assert.NotNull(result);
            Assert.Equal(tag.SlaveId, result.TagId); // 1
            Assert.Equal(tag.Name, result.TagName); // My tag name

            Assert.Equal(TimeInterval.ThisYear, result.Interval);
            Assert.Equal(startOfYear, result.IntervalFrom);
            Assert.Equal(today.AddHours(23).AddMinutes(59).AddSeconds(59), result.IntervalTo);

            Assert.Equal(0d, result.Minimum.Temperature);
            Assert.Equal(today.AddHours(1), result.Minimum.Timestamp);

            Assert.Equal(100d, result.Maximum.Temperature);
            Assert.Equal(today.AddHours(2), result.Maximum.Timestamp);
        }

        private TagInfo CreateTagInfo()
        {
            return new TagInfo()
            {
                SlaveId = 1,
                Name = "My tag name",
                Comment = "Description text",
                Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Temperature = 21.5,
                RelativeHumidity = 50,
                BatteryVoltage = 1.5,
                BatteryRemaining = 0.75,
                SignalStrength = 42,
                LastCommunication = new DateTime(2022, 1, 1),
                TagType = TagInfo.TemperatureTag
            };
        }

        private Measurement CreateTemperatureDataPoint(DateTime dateTime, double temperature)
        {
            return new Measurement(dateTime, temperature);
        }
    }
}
