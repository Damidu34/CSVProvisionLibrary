using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    public class CsvToListInserter<T> where T : new()
    {
        private readonly string filePath;
        private readonly char customDelimiter;

        public CsvToListInserter(string filePath, char customDelimiter)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            this.customDelimiter = customDelimiter;
        }

        public bool InsertCsvData(List<T> dataList)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            try
            {
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

                        var dataObject = new T();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            string propertyName = headers[i].Trim();
                            string propertyValue = i < values.Length ? values[i].Trim() : string.Empty;

                            PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (property != null)
                            {
                                // Convert the property value to the appropriate type
                                object convertedValue = Convert.ChangeType(propertyValue, property.PropertyType);
                                property.SetValue(dataObject, convertedValue);
                            }
                        }

                        dataList.Add(dataObject);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them as needed
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
                return false;
            }
        }
    }
}
