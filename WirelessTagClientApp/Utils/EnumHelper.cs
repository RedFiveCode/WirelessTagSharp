using System;
using System.Collections.Generic;
using System.Linq;


namespace WirelessTagClientApp.Utils
{
    public class EnumHelper
    {
        /// <summary>
        /// Get next value in enumeration, wrapping round
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp
        /// </remarks>
        public static T NextEnum<T>(T currentValue) where T : struct
        {
            List<T> values = GetEnumValues<T>();
            var i = values.FindIndex(n => currentValue.Equals(n)) + 1;

            return (i == values.Count() ? values.First() : values[i]);
        }

        // <summary>
        /// Get previous value in enumeration, wrapping round
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static T PreviousEnum<T>(T currentValue) where T : struct
        {

            List<T> values = GetEnumValues<T>();
            var i = values.FindIndex(n => currentValue.Equals(n)) - 1;

            return (i < 0 ? values.Last() : values[i]);
        }

        private static List<TEnumValue> GetEnumValues<TEnumValue>() where TEnumValue : struct
        {
            var type = typeof(TEnumValue);

            if (!type.IsEnum)
            {
                throw new ArgumentException($"Argument '{type.FullName}' is not an Enum type");
            }

            return Enum.GetValues(type).Cast<TEnumValue>().ToList();
        }
    }
}
