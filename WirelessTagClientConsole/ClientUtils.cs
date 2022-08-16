using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WirelessTagClientLib;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientConsole
{
    internal class ClientUtils
    {
        public static List<TagTemperatureSpan> GetRecentTemperatures(IWirelessTagAsyncClient client, List<TagInfo> tags, DateTime from, DateTime to)
        {
            var results = new List<TagTemperatureSpan>();

            foreach (var tag in tags.OrderBy(t => t.SlaveId))
            {
                var infoListTask = client.GetTemperatureRawDataAsync(tag.SlaveId, from, to);
                Task.WaitAll(infoListTask);

                var infoList = infoListTask.Result;

                if (infoList.Any())
                {
                    var min = infoList.Min(r => r.Temperature);
                    var max = infoList.Max(r => r.Temperature);

                    var result = new TagTemperatureSpan() { Tag = tag, Min = min, Max = max };
                    results.Add(result);
                }
                else
                {
                    // some tag may not have results for time interval, for example if they are broken and have not sent any data recently
                }
            }

            return results;
        }
    }
}
