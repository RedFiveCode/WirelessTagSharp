using Humanizer;
using System;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    /// <summary>
    /// Format timespan from now in a friendly way
    /// for example "3 days ago"
    /// </summary>
    /// <remarks>
    /// See https://github.com/Humanizr/Humanizer
    /// </remarks>
    public class HumanizeTimeSpanValueConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                var dt = (DateTime)value;
                var span = DateTime.Now.Subtract(dt);

                return String.Format(Properties.Resources.DurationAgo, span.Humanize()); // for example "3 days ago"
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// ConvertBack
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
