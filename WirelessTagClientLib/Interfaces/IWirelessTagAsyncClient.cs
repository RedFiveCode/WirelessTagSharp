using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Encapsulates asynchronous basic access to a Wireless Tag
    /// with strongly typed result classes.
    /// </summary>
    public interface IWirelessTagAsyncClient
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="password">Password.</param>
        Task<bool> LoginAsync(string email, string password);

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        Task<List<TagInfo>> GetTagListAsync();

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<HourlyReadingInfo> GetTemperatureStatsAsync(int tagId);

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        Task<StatsSpanInfo> GetTagSpanStatsAsync(List<int> tagIds);

        /// <summary>
        /// Get the raw data for the specified tag for a date range
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<List<Measurement>> GetTemperatureRawDataAsync(int tagId, DateTime from, DateTime to);
    }
}

