using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    /// <summary>
    /// ValueConverter to convert a boolean to a visibility enumeration.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Get/set the <see cref="Visibility"/> to use if the converter value is true.
        /// </summary>
        public Visibility VisibilityTrue { get; set; }

        /// <summary>
        /// Get/set the <see cref="Visibility"/> to use if the converter value is false.
        /// </summary>        
        public Visibility VisibilityFalse { get; set; }

        /// <summary>
        /// <see cref="BoolVisibilityConverter"/> constructor.
        /// </summary>
        public BooleanToVisibilityConverter()
        {
            VisibilityTrue = Visibility.Visible;
            VisibilityFalse = Visibility.Collapsed;
        }

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = false;

            if (value is bool)
            {
                boolValue = (bool)value;
            }

            return boolValue ? VisibilityTrue : VisibilityFalse;    
        }

        /// <summary>
        /// ConvertBack
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
