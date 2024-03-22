using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class CustomCsvReader
    {
        private readonly string filePath;
        private readonly char customDelimiter;

        public CustomCsvReader(string filePath, char customDelimiter)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.customDelimiter = customDelimiter;
        }

        public IEnumerable<Dictionary<string, string>> ReadCsv()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            try
            {
                var records = new List<Dictionary<string, string>>();

                using (var reader = new StreamReader(filePath))
                {
                    var headerLine = reader.ReadLine();
                    if (headerLine == null)
                    {
                        throw new InvalidOperationException("The file does not contain a header.");
                    }

                    var headers = headerLine.Split(customDelimiter);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(customDelimiter);

                        var record = new Dictionary<string, string>();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            string value = i < values.Length ? values[i] : string.Empty;
                            record[headers[i]] = value;
                        }

                        records.Add(record);
                    }
                }

                return records;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them as needed
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                return null;
            }
        }
    }
}
