using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class CsvStatusLogger
    {
        public void LogImported()
        {
            Log("Imported");
        }

        public void LogProcessing()
        {
            Log("Processing");
        }

        public void LogFinished()
        {
            Log("Finished");
        }

        public void LogError(string message)
        {
            Log($"Error: {message}");
        }

        private void Log(string status)
        {
            var formattedMessage = $"{DateTime.Now}: {status}";
            Console.WriteLine(formattedMessage);
        }
    }
}
