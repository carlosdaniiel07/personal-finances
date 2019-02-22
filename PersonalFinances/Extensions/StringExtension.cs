namespace System
{
    public static class StringExtension
    {
        public static string Cut (this string obj)
        {
            if (obj != null)
            {
                var maxStringLength = 24;
                return (obj.Length <= 24) ? obj : (obj.Substring(0, maxStringLength) + "...");
            }
            return null;
        }
    }
}