using System;
using System.Linq;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;
using WirelessTagClientLib.RawResponses;
using System.Globalization;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Factory class to convert json raw response classes to strongly typed DTO classes
    /// </summary>
    public class Mapper
    {
        public static StatsSpanInfo Create(StatsSpanData responseData)
        {
            var result = new StatsSpanInfo()
            {
                From = DateTime.FromFileTime(responseData.From),
                To = DateTime.FromFileTime(responseData.To),
                TimeZoneOffset = responseData.Tzo,
                Ids = new List<int>(responseData.Ids),
                Names = new List<string>(responseData.Names)
            };

            return result;
        }

        public static List<TagInfo> Create(TagListResponse responseData)
        {
            return responseData.Data.Select(item => Create(item))
                                    .OrderBy(t => t.SlaveId)
                                    .ToList();
        }

        public static List<Measurement> Create(TemperatureRawDataResponse response)
        {
            return response.Data.Select(item => Create(item))
                                .OrderBy(t => t.Time)
                                .ToList();
        }

        public static HourlyReadingInfo Create(TemperatureStatsResponse response)
        {
            var result = new HourlyReadingInfo()
            {
                TemperatureUnits = response.Data.TempUnit,
                HourlyReadings = response.Data.Temps.Select(item => Create(item))
                                                    .OrderBy(t => t.Date)
                                                    .ToList()
            };

            return result;
        }


        private static TagInfo Create(TagListData data)
        {
            var result = new TagInfo()
            {
                Name = data.Name,
                Uuid = Guid.Parse(data.Uuid),
                Comment = data.Comment,
                SlaveId = data.SlaveId,
                TagType = data.TagType,
                LastCommunication = DateTime.FromFileTime(data.LastComm),
                SignalStrength = data.SignaldBm,
                BatteryVoltage = data.BatteryVolt,
                BatteryRemaining = data.BatteryRemaining,
                Temperature = data.Temperature,
                ImageMD5 = data.ImageMd5,
                RelativeHumidity = data.Cap,
                IsOutOfRange = data.OutOfRange,
            };

            return result;
        }

        private static Measurement Create(TemperatureRawDataItem data)
        {
            var result = new Measurement()
            {
                //data.Type is not used
                Time = data.Time,
                Temperature = data.TempDegC,
                Humidity = data.Cap,
                Lux = data.Lux,
                Battery = data.BatteryVolts
            };

            return result;
        }

        private static HourlyReading Create(Temp data)
        {
            var result = new HourlyReading()
            {
                Date = DateTime.Parse(data.Date, new CultureInfo("en-US")), // "MM/DD/YYYY"
                Temperatures = data.Temps,
                Humidities = data.Caps
            };

            return result;
        }
    }
}
