using System;
using System.Collections.Generic;

namespace WirelessTagClientLib
{
    /// <summary>
    /// Encapsulates synchronous basic access to a Wireless Tag
    /// with raw json response strings.
    /// </summary>
    public interface IWirelessTagJsonClient
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <param name="password">Password.</param>
        string Login(string email, string password);

        /// <summary>
        /// Returns true if logged in.
        /// </summary>
        /// <returns></returns>
        string IsLoggedIn();

        /// <summary>
        /// Get a list of tags
        /// </summary>
        /// <returns></returns>
        string GetTagList();

        /// <summary>
        /// Get average hourly temperature for the specified tag
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        string GetTemperatureStats(int tagId);

        /// <summary>
        /// Get the date of the earliest and latest available data for a list of tags
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        string GetTagSpanStats(List<int> tagIds);

        /// <summary>
        /// Get the raw data for the specified tag for a date range
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        string GetTemperatureRawData(int tagId, DateTime from, DateTime to);

        /// <summary>
        /// Get the raw temperature/battery/humidity data for the specified tag for a date range
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        string GetTagStatsRawData(int tagId, DateTime from, DateTime to);
    }
}

