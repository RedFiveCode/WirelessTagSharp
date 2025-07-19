using System;
using System.Linq;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.ViewModels
{
    /// <summary>
    /// Data cache for temperature measurements, indexed by tagId.
    /// </summary>
    public class TemperatureRawDataCache
    {
        private Dictionary<int, HashSet<TemperatureDataPoint>> rawDataMap;
        private TemperatureDataPointComparer comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureRawDataCache"/> class
        /// </summary>
        public TemperatureRawDataCache()
        {
            rawDataMap = new Dictionary<int, HashSet<TemperatureDataPoint>>();
            comparer = new TemperatureDataPointComparer();
        }

        /// <summary>
        /// Updates the cache with new temperature data for the specified tagId.
        /// </summary>
        public void Update(int tagId, IEnumerable<TemperatureDataPoint> data)
        {
            // FYI
            // GetTemperatureRawDataAsync can return duplicate items
            // For example:
            //[0]: Time ={ 20-Jul-2015 13:32:02}, Temperature = 20.460712432861328, Humidity = 61.90924072265625, Lux = -1, Battery = 2.6572811200207851
            //[1]: Time ={ 20-Jul-2015 13:32:02}, Temperature = 20.460712432861328, Humidity = 61.9168701171875, Lux = -1, Battery = 2.6572811200207851

            //[0]: Time ={ 18-Aug-2015 13:38:41}, Temperature = 20.964792251586914, Humidity = 56.5, Lux = -1, Battery = 2.4708389506362045
            //[1]: Time ={ 18-Aug-2015 13:38:41}, Temperature = 20.975517272949219, Humidity = 56.49237060546875, Lux = -1, Battery = 2.470835032645573

            // Underlying json response data is
            //{
            //            "__type": "MyTagList.ethLogs+TemperatureDataPoint",
            //  "time": "2015-07-20T13:32:02+01:00",
            //  "temp_degC": 20.460712432861328,
            //  "cap": 61.90924072265625,
            //  "lux": -1,
            //  "battery_volts": 2.6572811200207851
            //}
            //{
            //  "__type": "MyTagList.ethLogs+TemperatureDataPoint",
            //  "time": "2015-07-20T13:32:02+01:00",
            //  "temp_degC": 20.460712432861328,
            //  "cap": 61.9168701171875,
            //  "lux": -1,
            //  "battery_volts": 2.6572811200207851
            //},

            //var duplicates = data.GroupBy(x => x.Time)
            //                    .Where(x => x.Count() > 1)
            //                    .Select(x => x.Key)
            //                    .ToList();

            //Console.WriteLine($"Tag {tagId} : Duplicates {duplicates.Count}");
            //if (duplicates.Count > 0)
            //{
            //    Console.WriteLine("Duplicates");
            //}

            // these duplicates will be filtered out by the respective HashSet<TemperatureDataPoint> with the TemperatureDataPointComparer comparer

            if (!rawDataMap.ContainsKey(tagId))
            {
                rawDataMap.Add(tagId, new HashSet<TemperatureDataPoint>(comparer));
            }

            var rawDataSet = rawDataMap[tagId];

            var originalItem = rawDataSet.ToList();

            //Console.WriteLine($"Tag {tagId} : Update start: current {rawDataSet.Count()}, merging {data.Count()}");

            //var commonItems = originalItem.Intersect(data).ToList();
            //var differenceItems = originalItem.Except(data).ToList();

            //Console.WriteLine($"Tag {tagId} : Before common {commonItems.Count}, differences {differenceItems.Count}");

            // add to existing set of values; no duplicates (set)
            foreach (var item in data)
            {
                rawDataSet.Add(item);
            }

            //var commonItems2 = rawDataSet.Intersect(data).ToList();
            //var differenceItems2 = rawDataSet.Except(data).ToList();

            //Console.WriteLine($"Tag {tagId} : After common {commonItems2.Count}, differences {differenceItems2.Count}");

            //Console.WriteLine($"Tag {tagId} : Update finish: now {rawDataSet.Count()}");
        }

        /// <summary>
        /// Returns the raw temperature data for the specified tagId
        /// </summary>
        public IEnumerable<TemperatureDataPoint> GetData(int tagId)
        {
            return rawDataMap[tagId];
        }

        /// <summary>
        /// Returns the all raw temperature data for all tags.
        /// </summary>
        public IEnumerable<TagMeasurementDataPoint> GetAllData()
        {
            return rawDataMap.SelectMany(kvp => kvp.Value.Select(item => new TagMeasurementDataPoint(kvp.Key, item.Time, item.Temperature, item.Humidity, item.Lux, item.Battery)));
        }

        /// <summary>
        /// Returns true if the cache contains data for the specified tagId
        /// </summary>
        public bool ContainsDataForTag(int tagId)
        {
            return rawDataMap.ContainsKey(tagId);
        }

        /// <summary>
        /// Returns true if the cache contains data for the specified tagId and date
        /// </summary>
        public bool ContainsDataForTag(int tagId, DateTime date)
        {
            if (!rawDataMap.ContainsKey(tagId))
            {
                return false;
            }

            return rawDataMap[tagId].Any(d => d.Time.Date == date.Date);
        }

        /// <summary>
        /// Returns true if the cache contains data for the specified tagId and date range
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool ContainsDataForTag(int tagId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
            {
                throw new ArgumentOutOfRangeException("From date must be less than or equal to To date.");
            }

            if (!rawDataMap.ContainsKey(tagId))
            {
                return false;
            }

            var start = from.Date;
            var finish = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // to be inclusive need to extend to the end of the day

            return rawDataMap[tagId].Any(d => d.Time >= start && d.Time <= finish);
        }

        /// <summary>
        /// Returns the number of tags in the cache.
        /// </summary>
        public int Count
        {
            get { return rawDataMap.Count; }
        }

        /// <summary>
        /// Returns the total number of measurements in the cache across all tags
        /// </summary>
        public int ItemCount
        {
            get { return rawDataMap.Sum(x => x.Value.Count); }
        }
    }
}
