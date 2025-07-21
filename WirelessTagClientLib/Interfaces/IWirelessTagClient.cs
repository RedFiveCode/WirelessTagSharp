using System;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Encapsulates synchronous basic access to a Wireless Tag
    /// with strongly typed result classes.
    /// </summary>
    public interface IWirelessTagClient
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="password">Password.</param>
        bool Login(string email, string password);

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        List<TagInfo> GetTagList();

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        HourlyReadingInfo GetTemperatureStats(int tagId);

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        StatsSpanInfo GetTagSpanStats(List<int> tagIds);

        /// <summary>
        /// Get the raw data for the specified tag for a date range
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        List<Measurement> GetTemperatureRawData(int tagId, DateTime from, DateTime to);
    }
}

