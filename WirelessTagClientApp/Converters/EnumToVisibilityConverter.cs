using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    /// <summary>
    /// Value converter to matching enumeration value to visibility
    /// </summary>
    public class EnumToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public EnumToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        #region IValueConverter Members

        /// <summary>
        /// Convert enum to visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null || !(parameter is string))
            {
                return FalseValue;
            }

            var t = value.GetType();

            if (!t.IsEnum || !Enum.IsDefined(t, value))
            {
                return FalseValue;
            }

            // convert parameter from string to enum type
            string parameterString = parameter as string;
            if (!Enum.IsDefined(t, parameterString))
            {
                return FalseValue; // not a value in the enumeration
            }

            var parameterValue = Enum.Parse(t, parameterString);

            return parameterValue.Equals(value) ? TrueValue : FalseValue;
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
        #endregion       
    }
}
