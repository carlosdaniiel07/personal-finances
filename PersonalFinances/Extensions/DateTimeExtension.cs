namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Get a date string (dd/MM/yyyy) from a DateTime object. If DateTime's ticks equals 0, then return a empty string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToShortDateStringDefaultIfNull (this DateTime obj)
        {
            return (obj.Ticks.Equals(0L)) ? "" : obj.ToShortDateString();
        }
    }
}