using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    public class TimeIntervalValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var map = CreateLookupTable();

            if (value is TimeInterval)
            {
                var interval = (TimeInterval)value;

                if (map.ContainsKey(interval))
                {
                    return map[interval];
                }
            }

            // wrong type or value not found
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Dictionary<TimeInterval, string> CreateLookupTable()
        {
            var dt = new DateTime(DateTime.Now.Year, 1, 1); // start of year
            string fmt = Properties.Resources.TimeInterval_CurrentMonth;

            var map = new Dictionary<TimeInterval, string>()
            {
                { TimeInterval.Today, Properties.Resources.TimeInterval_Today },
                { TimeInterval.Yesterday, Properties.Resources.TimeInterval_Yesterday },
                { TimeInterval.Last7Days, Properties.Resources.TimeInterval_Last7Days },
                { TimeInterval.Last30Days, Properties.Resources.TimeInterval_Last30Days },
                { TimeInterval.All, Properties.Resources.TimeInterval_All },

                // format to show year for example "2022"      
                { TimeInterval.ThisYear, string.Format(Properties.Resources.TimeInterval_ThisYear, dt) },

                // format to show month and current year for example "January 2022"      
                { TimeInterval.January,   dt.AddMonths(0).ToString(fmt) },
                { TimeInterval.February,  dt.AddMonths(1).ToString(fmt) },
                { TimeInterval.March,     dt.AddMonths(2).ToString(fmt) },
                { TimeInterval.April,     dt.AddMonths(3).ToString(fmt) },
                { TimeInterval.May,       dt.AddMonths(4).ToString(fmt) },
                { TimeInterval.June,      dt.AddMonths(5).ToString(fmt) },
                { TimeInterval.July,      dt.AddMonths(6).ToString(fmt) },
                { TimeInterval.August,    dt.AddMonths(7).ToString(fmt) },
                { TimeInterval.September, dt.AddMonths(8).ToString(fmt) },
                { TimeInterval.October,   dt.AddMonths(9).ToString(fmt) },
                { TimeInterval.November,  dt.AddMonths(10).ToString(fmt) },
                { TimeInterval.December,  dt.AddMonths(11).ToString(fmt) },
            };

            return map;
        }

    }
}
