using System;

namespace WirelessTagClientApp.Utils
{
    /// <summary>
    /// Class to convert a <see cref="TimeInterval"/> interval into a range of dates.
    /// </summary>
    public class TimeIntervalHelper
    {
        /// <summary>
        /// Get date range based on starting date and interval.
        /// </summary>
        /// <param name="dt">Starting date</param>
        /// <param name="interval">Interval</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Tuple<DateTime, DateTime> GetTimeRange(DateTime dt, TimeInterval interval)
        {
            if (interval == TimeInterval.Today)
            {
                var from = dt.Date;
                var to = from.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.Yesterday) // yesterday but not today
            {
                var from = dt.Date.AddDays(-1);
                var to = from.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.Last7Days)
            {
                var from = dt.Date.AddDays(-7);
                var to = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.Last30Days)
            {
                var from = dt.Date.AddDays(-30);
                var to = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.ThisYear)
            {
                var from = new DateTime(dt.Year, 1, 1);
                var to = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.January)
            {
                var from = new DateTime(dt.Year, 1, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.February)
            {
                var from = new DateTime(dt.Year, 2, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.March)
            {
                var from = new DateTime(dt.Year, 3, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.April)
            {
                var from = new DateTime(dt.Year, 4, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.May)
            {
                var from = new DateTime(dt.Year, 5, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.June)
            {
                var from = new DateTime(dt.Year, 6, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.July)
            {
                var from = new DateTime(dt.Year, 7, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.August)
            {
                var from = new DateTime(dt.Year, 8, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.September)
            {
                var from = new DateTime(dt.Year, 9, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.October)
            {
                var from = new DateTime(dt.Year, 10, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.November)
            {
                var from = new DateTime(dt.Year, 11, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.December)
            {
                var from = new DateTime(dt.Year, 12, 1);
                var to = from.Date.AddMonths(1).AddSeconds(-1);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.All)
            {
                var from = new DateTime(2000, 1, 1); // arbitrary start of epoch
                var to = dt;

                return new Tuple<DateTime, DateTime>(from, to);
            }

            // All
            throw new ArgumentOutOfRangeException($"TimeInterval enum value {interval} is not supported");
        }
    }


}
