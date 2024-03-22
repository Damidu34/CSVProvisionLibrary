using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;


  public class ObjectCollectionCsvValidator<T>
    {
        private readonly CsvConfiguration csvConfiguration;

        public ObjectCollectionCsvValidator(char delimiter = ',')
        {
            csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter.ToString(),
                HasHeaderRecord = true,
            };
        }

        public bool ValidateObjectCollection(List<T> objects, Func<T, bool> dataValidation)
        {
            try
            {
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, csvConfiguration))
                {
                    // Write objects to CSV format in-memory
                    csv.WriteRecords(objects);
                    writer.Flush();

                    var csvString = writer.ToString();
                    var csvReader = new StringReader(csvString);

                    // Read and validate data
                    using (var dataCsv = new CsvReader(csvReader, csvConfiguration))
                    {
                        while (dataCsv.Read())
                        {
                            var record = dataCsv.GetRecord<T>();

                            // Validate data using the provided criteria
                            if (!dataValidation(record))
                            {
                                Console.WriteLine("CSV data does not meet the validation criteria.");
                                return false;
                            }
                        }
                    }

                    Console.WriteLine("CSV validation successful.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating CSV data: {ex.Message}");
                return false;
            }
        }
    }

