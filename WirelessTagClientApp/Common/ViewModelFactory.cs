using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoreLinq;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Common
{
    /// <summary>
    /// Creates view-model objects from DTO objects
    /// </summary>
    public class ViewModelFactory
    {
        public static TagViewModel CreateTagViewModel(TagInfo tag)
        {
            var tagVM = new TagViewModel()
            {
                Id = tag.SlaveId,
                Name = tag.Name,
                Description = tag.Comment,
                Uuid = tag.Uuid,
                LastCommunication = tag.LastCommunication,
                Temperature = tag.Temperature,
                SignalStrength = tag.SignalStrength,
                BatteryVoltage = tag.BatteryVoltage,
                BatteryRemaining = 100d * tag.BatteryRemaining, // convert to percentage 0 to 100, not 0 to 1
                RelativeHumidity = tag.RelativeHumidity,
                IsHumidityTag = tag.IsHumidityTag
            };

            return tagVM;
        }

        public static ObservableCollection<TagViewModel> CreateTagViewModelList(IEnumerable<TagInfo> tags)
        {
            return CreateTagViewModelList(tags, TagViewModel.ViewMode.Temperature);
        }

        public static ObservableCollection<TagViewModel> CreateTagViewModelList(IEnumerable<TagInfo> tags, TagViewModel.ViewMode mode)
        {
            var collection = new ObservableCollection<TagViewModel>();

            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    var item = CreateTagViewModel(tag);
                    item.Mode = mode;
                        
                    collection.Add(item);
                }
            }

            return collection;
        }

        public static MinMaxMeasurementViewModel CreateRowViewModel(IEnumerable<TemperatureDataPoint> rawData, TagInfo tag, TimeInterval interval)
        {
            if (rawData == null || !rawData.Any())
            {
                return null; // no data points
            }

            var timeRange = TimeIntervalHelper.GetTimeRange(DateTime.Now, interval);

            var dataPointsInTimeInterval = rawData.Where(d => d.Time >= timeRange.Item1 && d.Time <= timeRange.Item2);

            if (!dataPointsInTimeInterval.Any())
            {
                return null; // no data points within time range
            }

            var coldest = dataPointsInTimeInterval.MinBy(tdp => tdp.Temperature).First();
            var warmest = dataPointsInTimeInterval.MaxBy(tdp => tdp.Temperature).First();

            var row = new MinMaxMeasurementViewModel()
            {
                Interval = interval,
                IntervalFrom = timeRange.Item1,
                IntervalTo = timeRange.Item2,
                TagId = tag.SlaveId,
                TagName = tag.Name,
                Minimum = new Measurement(coldest.Temperature, coldest.Time),
                Maximum = new Measurement(warmest.Temperature, warmest.Time),
                Count = dataPointsInTimeInterval.Count()
            };

            return row;
        }
    }
}
