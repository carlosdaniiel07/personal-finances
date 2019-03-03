using System.IO;
using System.Text;

using PersonalFinances.Models;

namespace PersonalFinances.Services
{
    public class LoggingService
    {
        /// <summary>
        /// Log an error in a txt file
        /// </summary>
        public void Log (Error error)
        {
            var tempPath = Path.GetTempPath();
            var fileName = $"Personal_Finances_ErrorLog_{error.When.ToString("ddMMyyyyHHmmss")}";
            var filePath = tempPath + Path.DirectorySeparatorChar + fileName + ".txt";

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        var stringBuilder = new StringBuilder();

                        stringBuilder.AppendLine($"[{error.When.ToString()}] => {error.Exception.Message}");
                        stringBuilder.AppendLine($"Target URL => {error.Url}");
                        stringBuilder.AppendLine($"Http method => {error.Method}");
                        stringBuilder.AppendLine($"User browser => {error.Browser}");
                        stringBuilder.AppendLine($"User IP => {error.UserAddress}");
                        stringBuilder.AppendLine($"Stack trace => {error.Exception.StackTrace}");
                        stringBuilder.AppendLine($"Method => {error.Exception.TargetSite}");

                        writer.Write(stringBuilder.ToString());
                    }
                }
            }
            catch (IOException)
            {

            }
        }
    }
}