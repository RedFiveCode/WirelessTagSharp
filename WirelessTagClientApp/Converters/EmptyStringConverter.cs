using System;
using System.Windows.Data;

namespace WirelessTagClientApp.Converters
{
    /// <summary>
    /// Value converter to return string text or "(none)" if value is null or empty.
    /// </summary>
    public class EmptyStringConverter : IValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyStringConverter"/> class.
        /// </summary>
        public EmptyStringConverter()
        {
            UnavailableText = Properties.Resources.UnavailableText;
        }

        /// <summary>
        /// Get/set the text to display if the value is not available.
        /// </summary>
        public string UnavailableText { get; set; }

        #region IValueConverter Members

        /// <summary>
        /// Convert the value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return UnavailableText;
            }

            string text = value.ToString();

            if (String.IsNullOrEmpty(text))
            {
                return UnavailableText;
            }

            return text;
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
