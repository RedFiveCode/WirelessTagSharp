using System;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.ViewModels
{
    public class TemperatureDataPointComparer : IEqualityComparer<TemperatureDataPoint>
    {
        public bool Equals(TemperatureDataPoint x, TemperatureDataPoint y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Time == y.Time;
        }

        public int GetHashCode(TemperatureDataPoint p)
        {
            return p.Time.GetHashCode();
        }
    }
}
