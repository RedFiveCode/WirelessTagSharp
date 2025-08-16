using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientAppTest.TestHelpers;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.Commands
{
    
    public class RefreshMinMaxTagsCommandTest
    {
        private readonly Mock<IWirelessTagAsyncClient> _clientMock;
        private readonly RefreshMinMaxTagsCommand _target;

        public RefreshMinMaxTagsCommandTest()
        {
            // Ensure we have a SynchronizationContext for task continuations in the view-model;
            // WPF has this by default, but unit tests do not, otherwise we get an InvalidOperationException
            // "The current SynchronizationContext may not be used as a TaskScheduler"
            // See https://stackoverflow.com/questions/8245926/the-current-synchronizationcontext-may-not-be-used-as-a-taskscheduler
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            _clientMock = CreateAsyncClientMock();
            _target = new RefreshMinMaxTagsCommand(_clientMock.Object);
        }

        [Fact]
        public void Command_Implements_ICommand()
        {
            Assert.IsAssignableFrom<ICommand>(_target.Command);
        }

        [Fact]
        public void CanExecute_Returns_True()
        {
            // arrange
            var viewModel = new MinMaxViewModel();

            // act
            var result = _target.Command.CanExecute(viewModel);

            // assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Call_WirelessTagClient_GetTagListAsync()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // act
            await _target.ExecuteAsync(viewModel);

            // assert
            _clientMock.Verify(x => x.GetTagListAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_NoTags_Should_NotCall_GetTemperatureRawDataAsync()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            _clientMock.Setup(x => x.GetTagListAsync())
                      .ReturnsAsync(new List<TagInfo>()); // empty list

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; verify that GetTemperatureRawDataAsync is never called
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Call_GetTemperatureRawDataAsync_ForEachTag()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                            // return different data per tag

                            if (id == 1)
                            {
                                return new List<Measurement>()
                                {
                                    new Measurement(new DateTime(2025, 8, 15, 8, 0, 0), 8d),
                                    new Measurement(new DateTime(2025, 8, 15, 10, 0, 0), 10d)
                                };
                            }
                            else if (id == 2)
                            {
                                // Mock data for tag 1
                                return new List<Measurement>()
                                {
                                    new Measurement(new DateTime(2025, 8, 12, 7, 0, 0), 15d),
                                    new Measurement(new DateTime(2025, 8, 12, 8, 0, 0), 12d)
                                };
                            }

                          return new List<Measurement>() { }; // return empty list for other tags
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; verify that GetTemperatureRawDataAsync is called for each of the two tags
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(2, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Call_GetTemperatureRawDataAsync_ForToday()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; verify that GetTemperatureRawDataAsync is called for each of the two tags
            var expectedFrom = DateTime.Today.Date; // start of today
            var expectedTo = expectedFrom.AddHours(23).AddMinutes(59).AddSeconds(59); // end of today

            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(1, expectedFrom, expectedTo), Times.AtLeastOnce);
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(2, expectedFrom, expectedTo), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Call_GetTemperatureRawDataAsync_ForYesterdayAndEarlier()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; verify that GetTemperatureRawDataAsync is called for each of the two tags
            var today = DateTime.Today.Date;

            var expectedFrom = new DateTime(today.Year, 1, 1); // start of this year
            var expectedTo = today.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59); // end of yesterday

            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(1, expectedFrom, expectedTo), Times.AtLeastOnce);
            _clientMock.Verify(x => x.GetTemperatureRawDataAsync(2, expectedFrom, expectedTo), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_ForEachTag()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          // return different data per tag; order should not be important, but likely acquired in time order

                          var today = DateTime.Today.Date; // start of today

                          var yesterday = today.AddDays(-1); // end of yesterday

                          // TODO data should not be in the future, especially today's data

                          if (id == 1)
                          {
                                return new List<Measurement>()
                                {
                                    new Measurement(yesterday.AddHours(2), 2), // minimum for yesterday
                                    new Measurement(yesterday.AddHours(3), 3), // maximum for yesterday

                                    new Measurement(today.AddHours(4), 8), // minimum for today
                                    new Measurement(today.AddHours(5), 9),
                                    new Measurement(today.AddHours(6), 10) // maximum for today
                                };
                          }
                          else if (id == 2)
                          {
                                // Mock data for tag 1
                                return new List<Measurement>()
                                {
                                    new Measurement(yesterday.AddHours(4), 4), // minimum for yesterday
                                    new Measurement(yesterday.AddHours(5), 5),
                                    new Measurement(yesterday.AddHours(6), 6), // maximum for yesterday

                                    new Measurement(today.AddHours(4), 15), // maximum for today
                                    new Measurement(today.AddHours(5), 13), // minimum for today
                                    new Measurement(today.AddHours(6), 12)  // minimum for today
                                };
                          }

                          return new List<Measurement>() { }; // return empty list for other tags
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval

            // should have 12 items in total; 2 tags * 6 intervals = 12 items
            Assert.Equal(12, viewModel.Data.Count);

            // should have two tags
            var uniqueTags = viewModel.Data.Select(d => d.TagId).Distinct().ToList();

            Assert.Equivalent(new int[] { 1, 2 }, uniqueTags); // should have two unique tags

            // should have 4 time intervals; today and earlier time buckets
            var uniqueIntervals = viewModel.Data.Select(d => d.Interval).Distinct().ToList();
            Assert.Equivalent(new TimeInterval[] { TimeInterval.Today, TimeInterval.Yesterday, TimeInterval.Last7Days, TimeInterval.Last30Days, TimeInterval.ThisYear }, uniqueIntervals);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_TodayOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var today = DateTime.Today.Date; // start of today

            // today 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(today, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertContainsDataForTag(viewModel, tagId, TimeInterval.Today, minTemperature, maxTemperature);
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday); // no data for yesterday, as we only have today's data
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last7Days, minTemperature, maxTemperature); // today is included in the last 7, 30 etc days
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last30Days, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.ThisYear, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_YesterdayOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var yesterday = DateTime.Today.Date.AddDays(-1); // start of yesterday

            // yesterday: 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(yesterday, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Today); // no data for today, as we only have yesterday's data
            AssertContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday, minTemperature, maxTemperature); // data for yesterday
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last7Days, minTemperature, maxTemperature); // yesterday is included in the last 7, 30 etc days
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last30Days, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.ThisYear, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_ThisWeekOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var thisWeek = DateTime.Today.Date.AddDays(-3); // sometime in the last week

            // a day this week: 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(thisWeek, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                       .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Today); // no data for today
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday); // no data for yesterday
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last7Days, minTemperature, maxTemperature); // the day this week is included in the last 7, 30 etc days
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last30Days, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.ThisYear, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_ThisMonthOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // return different data per interval; order should not be important, but likely acquired in time order

            var thisMonth = DateTime.Today.Date.AddDays(-10); // sometime in the last month, but not today, yesterday or last week

            // 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(thisMonth, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                       .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                       .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Today); // no data for today
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday); // no data for yesterday
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.Last7Days);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.Last30Days, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.ThisYear, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_ThisYearOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var thisYear = DateTime.Today.Date.AddDays(-50); // sometime this year, but not today, yesterday, last week or last month

            // ensure we don't drop into the previous year if running the test within the first 50 days of the year
            if (thisYear.Year < DateTime.Today.Year)
            {
                thisYear = new DateTime(DateTime.Today.Year, 1, 1); // start of this year
            }

            // 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(thisYear, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                       .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                       .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Today); // no data for today
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday); // no data for yesterday
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.Last7Days);
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.Last30Days);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.ThisYear, minTemperature, maxTemperature);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_TimeIntervalMaxMin_LastYearOnly()
        {
            const int tagId = 1;
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // return different data per interval; order should not be important, but likely acquired in time order

            var lastYear = new DateTime(DateTime.Today.Year, 1, 1); // start of this year
            lastYear = lastYear.AddDays(-50); // sometime last year, but not today, yesterday, last week, last month, or this year

            // 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(lastYear, minTemperature, maxTemperature);

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                       .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = tagId, Name = "My tag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                       .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            // act
            await _target.ExecuteAsync(viewModel);

            // assert; the view model's Data property should be updated
            // this contains the min-max measurements for each tag for each time interval
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Today); // no data for today
            AssertNotContainsDataForTag(viewModel, tagId, TimeInterval.Yesterday); // no data for yesterday
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.Last7Days);
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.Last30Days);
            AssertNotContainsDataForTag(viewModel, 1, TimeInterval.ThisYear);
            AssertContainsDataForTag(viewModel, 1, TimeInterval.All, minTemperature, maxTemperature);
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_Cache_Today()
        {
            // Arrange
            var tagId = 42;
            var today = DateTime.Now.Date;

            // 12 @ 00:00:00, 13 @ 01:00:00, 14 @ 02:00:00
            var measurements = CreateMeasurementsList(today, 10, 12);

            _clientMock.Setup(x => x.GetTagListAsync()).ReturnsAsync(new List<TagInfo> { new TagInfo { SlaveId = tagId, Name = "TestTag" } });
            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(tagId, today, today.AddHours(23).AddMinutes(59).AddSeconds(59)))
                       .ReturnsAsync(measurements);

            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // Act
            await _target.ExecuteAsync(viewModel);

            // Assert
            AssertCacheContainsDataForTag(viewModel, tagId, measurements[0].Time, measurements[0].Temperature); // 10
            AssertCacheContainsDataForTag(viewModel, tagId, measurements[1].Time, measurements[1].Temperature); // 12
        }

        [Fact]
        public async Task ExecuteAsync_Should_UpdateViewModel_Cache_ThisYear()
        {
            const int minTemperature = 10;
            const int maxTemperature = 12;

            // Arrange
            var tagId = 42;
            var today = DateTime.Now.Date;

            // Create measurements for today, yesterday, this week, this month, and this year
            var measurements = new List<Measurement>();

            measurements.AddRange(CreateMeasurementsList(today, 30, 35));
            measurements.AddRange(CreateMeasurementsList(today.AddDays(-1), minTemperature, maxTemperature)); // yesterday
            measurements.AddRange(CreateMeasurementsList(today.AddDays(-3), minTemperature, maxTemperature)); // this week
            measurements.AddRange(CreateMeasurementsList(today.AddDays(-10), minTemperature, maxTemperature)); // this month
            measurements.AddRange(CreateMeasurementsList(new DateTime(today.Year, 1, 1), minTemperature, maxTemperature)); // this year
            measurements.AddRange(CreateMeasurementsList(new DateTime(today.Year - 1, 6, 30), 15, 20)); // last year (even older)

            Assert.DoesNotContain(measurements, m => m.Time > DateTime.Now); // data should not be in the future, especially today's data

            _clientMock.Setup(x => x.GetTagListAsync())
                       .ReturnsAsync(new List<TagInfo> { new TagInfo { SlaveId = tagId, Name = "TestTag" } });

            _clientMock.Setup(x => x.GetTemperatureRawDataAsync(tagId, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                       .ReturnsAsync((int id, DateTime from, DateTime to) =>
                      {
                          return measurements;
                      });

            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            // Act
            await _target.ExecuteAsync(viewModel);

            // Assert

            // all measurements should be in the cache
            Assert.All(measurements, m =>
            {
                AssertCacheContainsDataForTag(viewModel, tagId, m.Time, m.Temperature);
            });

            // the view model should contain data for all time interval buckets
            var intervals = new[]
            {
                TimeInterval.Today,
                TimeInterval.Yesterday,
                TimeInterval.Last7Days,
                TimeInterval.Last30Days,
                TimeInterval.ThisYear,
                TimeInterval.All
            };
            Assert.All(intervals, interval =>
            {
                AssertContainsDataForInterval(viewModel, tagId, interval);
            });
        }

        [Fact]
        public async Task ExecuteAsync_Should_Update_Data_Property()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            bool onCollectionChanged = false;
            viewModel.Data.CollectionChanged += (sender, args) => { onCollectionChanged = true; };

            // act
            await _target.ExecuteAsync(viewModel);

            // assert
            Assert.True(onCollectionChanged);
        }

        [Fact]
        public async Task ExecuteAsync_Sets_LastUpdated_Property_OnParentViewModel()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var observer = new PropertyChangedObserver(parentViewModel);

            // act - await async operations returning
            await _target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("LastUpdated");
        }

        [Fact]
        public async Task ExecuteAsync_Sets_IsBusy_Property_OnParentViewModel()
        {
            // arrange
            var parentViewModel = new MainWindowViewModel();
            var viewModel = new MinMaxViewModel(parentViewModel);

            var observer = new PropertyChangedObserver(parentViewModel);

            // act - await async operations returning
            await _target.ExecuteAsync(viewModel);

            // assert
            observer.AssertPropertyChangedEvent("IsBusy", 2);
        }

        private Mock<IWirelessTagAsyncClient> CreateAsyncClientMock()
        {
            var clientMock = new Mock<IWirelessTagAsyncClient>();

            clientMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
                 .ReturnsAsync(true);

            clientMock.Setup(x => x.GetTagListAsync())
                .ReturnsAsync(new List<TagInfo>() { new TagInfo() { SlaveId = 1, Name = "Tag one" }, new TagInfo() { SlaveId = 2, Name = "Tag two" } });

            clientMock.Setup(x => x.GetTemperatureRawDataAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                      .ReturnsAsync(new List<Measurement>()
                      {  
                          new Measurement(DateTime.Today.Date, 10d)
                      });

            return clientMock;
        }

        private void AssertCacheContainsDataForTag(MinMaxViewModel viewModel, int tagId, DateTime expectedTimestamp, double expectedTemperature)
        {
            var cached = viewModel.RawDataCache.GetData(tagId).ToList();
            Assert.NotNull(cached);
            Assert.True(cached.Count > 0);

            Assert.True(cached.Any(d => d.Time == expectedTimestamp && d.Temperature == expectedTemperature), $"Cache should contain data for tag {tagId} on {expectedTimestamp} with value {expectedTemperature}");
        }

        private void AssertContainsDataForInterval(MinMaxViewModel viewModel, int tagId, TimeInterval expectedTimeInterval)
        {
            Assert.True(viewModel.Data.Any(row => row.TagId == tagId && row.Interval == expectedTimeInterval), $"ViewModel should contain rows for tag {tagId} for {expectedTimeInterval}");
        }

        private void AssertContainsDataForTag(MinMaxViewModel viewModel, int tagId, TimeInterval expectedInterval, double expectedMinimumTemperature, double expectedMaximumTemperature)
        {
            Assert.True(viewModel.Data.Any(row => row.TagId == tagId &&
                                                  row.Interval == expectedInterval &&
                                                  row.Minimum.Temperature == expectedMinimumTemperature &&
                                                  row.Maximum.Temperature == expectedMaximumTemperature),
                                                  $"ViewModel should contain data for tag {tagId} for {expectedInterval} with minimum {expectedMinimumTemperature} and maximum {expectedMaximumTemperature}");
        }

        private void AssertNotContainsDataForTag(MinMaxViewModel viewModel, int tagId, TimeInterval expectedInterval)
        {
            Assert.False(viewModel.Data.Any(row => row.TagId == tagId && row.Interval == expectedInterval),
                         $"ViewModel should NOT contain data for tag {tagId} for {expectedInterval}");
        }

        private List<Measurement> CreateMeasurementsList(DateTime startTime, double minTemperature, double maxTemperature, double temperatureStep = 1, double timeStep = 1)
        {
            var today = startTime;

            var measurements = new List<Measurement>();

            for (double temperature = minTemperature, deltaHours = 0;
                        temperature <= maxTemperature;
                        temperature += temperatureStep, deltaHours += timeStep)
            {
                var date = today.AddHours(deltaHours);
                measurements.Add(new Measurement(date, temperature));
            }

            return measurements;
        }
    }
}
