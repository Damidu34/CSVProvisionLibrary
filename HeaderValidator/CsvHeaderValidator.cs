using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

namespace HelperLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
        public class CsvHeaderValidator
        {
            private readonly List<string> ExpectedHeaders;
            private readonly bool CaseInsensitive;

            public CsvHeaderValidator(List<string> expectedHeaders, bool caseInsensitive = false)
            {
                ExpectedHeaders = expectedHeaders ?? throw new ArgumentNullException(nameof(expectedHeaders));
                CaseInsensitive = caseInsensitive;
            }

            public bool ValidateCsvHeaders(string filePath, Func<string, bool> customValidationRule = null)
            {
                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"File not found: {filePath}");

                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var headerLine = reader.ReadLine();
                        if (headerLine == null)
                            throw new InvalidOperationException("The file does not contain a header.");

                        var actualHeaders = headerLine.Split(',');

                        // Check if the actual headers match the expected headers
                        var headersMatch = CaseInsensitive
                            ? actualHeaders.Select(header => header.Trim().ToLower()).SequenceEqual(ExpectedHeaders.Select(h => h.Trim().ToLower()))
                            : actualHeaders.Select(header => header.Trim()).SequenceEqual(ExpectedHeaders.Select(h => h.Trim()));

                        // Perform custom validation if a rule is provided
                        if (customValidationRule != null)
                        {
                            headersMatch = headersMatch && actualHeaders.All(customValidationRule);
                        }

                        return headersMatch;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions or log them as needed
                    Console.WriteLine($"Error validating CSV headers: {ex.Message}");
                    return false;
                }
            }
        }
    
}
