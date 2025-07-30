using System;
using System.Collections.Generic;

namespace WirelessTagClientLib
{
    /// <summary>
    /// DateTimeChunker
    /// </summary>
    public class DateTimeChunker
    {
        public static IEnumerable<(DateTime Start, DateTime End)> Foo()
        {
            throw new ArgumentException();
        }

        /// <summary>
        /// Returns a date range split into chunks of a maximum size
        /// </summary>
        /// <remarks>
        /// In the return tuple, the start value is rounded down to midnight (00:00:00),
        /// and the end value is rounded up to just before midnight (23:59:59).
        /// </remarks>
        /// <param name="from">From date</param>
        /// <param name="to">To date</param>
        /// <param name="interval">Maximum chunk size, typically days</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="from"/> is after <paramref name="to"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="interval"/> is less than or equal to zero</exception>
        public static List<(DateTime Start, DateTime End)> SplitDateTimeRange(DateTime from, DateTime to, TimeSpan interval)
        {
            if (from > to)
            {
                throw new ArgumentException("'from' date must be less than or equal to 'to' date", nameof(from));
            }

            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("Interval must be greater than zero", nameof(interval));
            }

            var results = new List<(DateTime Start, DateTime End)>();

            var currentStart = from.Date;
            var finish = to.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            while (currentStart <= finish)
            {
                var currentEnd = currentStart.Add(interval).AddDays(-1); // inclusive range

                // If the calculated end exceeds the 'to' date, use 'to' as the end
                if (currentEnd > finish)
                {
                    currentEnd = finish;
                }

                // Round up the end time to 23:59:59.9999999 (end of day)
                currentEnd = currentEnd.Date.AddDays(1).AddTicks(-1);

                results.Add((currentStart, currentEnd));

                // Move to the next chunk start
                currentStart = currentEnd.AddTicks(1); // Add 1 tick to avoid overlap
            }

            return results;
        }
    }
}
