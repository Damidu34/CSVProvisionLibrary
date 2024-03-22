using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class ObjectToCsvWriteLibrary<T>
    {
        private readonly CsvConfiguration csvConfiguration;

        public ObjectToCsvWriteLibrary(char delimiter = ',')
        {
            this.csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter.ToString(),
                HasHeaderRecord = true,
            };
        }

        public void WriteCsv(string csvFilePath, IEnumerable<T> data, List<string> customHeaders = null)
        {
            try
            {
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, csvConfiguration))
                {
                    if (customHeaders != null)
                    {
                        foreach (var header in customHeaders)
                        {
                            csv.WriteField(header);
                        }
                        csv.NextRecord();
                    }

                    // Skip writing headers and data for the first item
                    var isFirstRow = false;
                    foreach (var item in data)
                    {
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        var properties = typeof(T).GetProperties();
                        foreach (var property in properties)
                        {
                            csv.WriteField(property.GetValue(item));
                        }
                        csv.NextRecord();
                    }
                }

                Console.WriteLine($"CSV data written to file: {csvFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing CSV file: {ex.Message}");
            }
        }
    }
}
