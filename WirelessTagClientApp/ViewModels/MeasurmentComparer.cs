using System;
using System.Collections.Generic;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.ViewModels
{
    public class MeasurmentComparer : IEqualityComparer<Measurement>
    {
        public bool Equals(Measurement x, Measurement y)
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

        public int GetHashCode(Measurement p)
        {
            return p.Time.GetHashCode();
        }
    }
}
