using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WirelessTagClientLib.DTO
{
    /// <summary>
    /// Strongly typed class storing result of GetTagSpanStats
    /// </summary>
    [DebuggerDisplay("From={From}, To={To}")]
    public class StatsSpanInfo
    {
        /// <summary>
        /// Earliest available data
        /// </summary>
        public DateTime From { get; internal set; }

        /// <summary>
        /// Latest available data
        /// </summary>
        public DateTime To { get; internal set; }

        /// <summary>
        /// Tag manager time zone offset (minutes)
        /// </summary>
        public int TimeZoneOffset { get; internal set; }

        /// <summary>
        /// List of tag ids
        /// </summary>
        public List<int> Ids { get; internal set; }

        /// <summary>
        /// List of tag names
        /// </summary>
        public List<string> Names { get; internal set; }
    }
}
