using System;

namespace WirelessTagClientApp.Utils
{
    /// <summary>
    /// Class to convert a <see cref="TimeInterval"/> interval into a range of dates.
    /// </summary>
    public class TimeIntervalHelper
    {
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

            if (interval == TimeInterval.Last7Days || interval == TimeInterval.Last30Days)
            {
                int daysAgo = (int)interval;
                var from = dt.Date.AddDays(-daysAgo);
                var to = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                return new Tuple<DateTime, DateTime>(from, to);
            }

            if (interval == TimeInterval.ThisYear)
            {
                var from = new DateTime(dt.Year, 1, 1);
                var to = dt.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

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
