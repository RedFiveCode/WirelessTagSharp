namespace WirelessTagClientApp.Utils
{
    public class TemperatureConvertor
    {
        /// <summary>
        /// Convert °C to °F.
        /// </summary>
        /// <param name="celcius"></param>
        /// <returns></returns>
        public static double ConvertToFarenheit(double celcius)
        {
            return 32d + ((9d * celcius) / 5d);
        }
    }
}
