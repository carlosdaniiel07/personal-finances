using System;

namespace PersonalFinances.Models
{
    public class Error
    {
        public string Browser { get; private set; }
        public string Url { get; private set; }
        public string Method { get; private set; }
        public string UserAddress { get; private set; }
        public DateTime When { get; private set; }
        public Exception Exception { get; private set; }

        public Error (string browser, string url, string method, string userAddress, Exception exception)
        {
            Browser = browser;
            Url = url;
            Method = method;
            UserAddress = userAddress;
            When = DateTime.Now;
            Exception = exception;
        }
    }
}