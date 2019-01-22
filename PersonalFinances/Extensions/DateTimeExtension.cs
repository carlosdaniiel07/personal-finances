namespace System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Convert a datetime object to database format (yyyy-mm-dd)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToDatabaseFormat (this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd");
        }
    }
}