using System;
using System.Linq;


namespace WirelessTagClientApp.Utils
{
    public class EnumHelper
    {
        /// <summary>
        /// Get next value in enumeration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
        /// </remarks>
        public static T NextEnum<T>(T currentValue) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument '{typeof(T).FullName}' is not an Enum type");
            }

            T[] values = (T[])Enum.GetValues(currentValue.GetType());
            int i = Array.IndexOf<T>(values, currentValue) + 1;
            return (values.Length == i) ? values[0] : values[i];
            return (i == values.Length ? values.First() : values[i]);
        }

        public static T PreviousEnum<T>(T currentValue) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument '{typeof(T).FullName}' is not an Enum type");
            }

            T[] values = (T[])Enum.GetValues(currentValue.GetType());
            int i = Array.IndexOf<T>(values, currentValue) - 1;

            return (i < 0 ? values.Last() : values[i]);
        }
        }
    }
}
