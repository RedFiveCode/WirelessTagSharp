using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    public class TimeIntervalValueConverter : IValueConverter
    {
        private Dictionary<TimeInterval, string> map = new Dictionary<TimeInterval, string>()
        {
            { TimeInterval.Today, Properties.Resources.TimeInterval_Today },
            { TimeInterval.Yesterday, Properties.Resources.TimeInterval_Yesterday },
            { TimeInterval.Last7Days, Properties.Resources.TimeInterval_Last7Days },
            { TimeInterval.Last30Days, Properties.Resources.TimeInterval_Last30Days },
            { TimeInterval.ThisYear, Properties.Resources.TimeInterval_ThisYear },
            { TimeInterval.All, Properties.Resources.TimeInterval_All }
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
    }
}
