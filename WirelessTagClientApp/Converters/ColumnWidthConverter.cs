using System;
using System.Globalization;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    /// <summary>
    /// This converter targets a column header, in order to take its width to zero when it should be hidden.
    /// </summary>
    /// <remarks>
    /// https://highfieldtales.wordpress.com/2013/08/05/hacking-gridview-hide-columns/
    /// </remarks>
    public class ColumnWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var isVisible = (bool)value;

                if (parameter != null)
                {
                    if (double.TryParse(parameter.ToString(), out double width))
                    {
                        return isVisible ? width : 0d;
                    }
                }
            }
            return 0d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
