using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLibrary
{
    
        public class CsvClassValidator
    {
        public List<T> ValidateCsvFileData<T>(string filePath, Dictionary<string, Type> classStructure)
        {
            var csvData = File.ReadAllLines(filePath);
            return ValidateCsvData<T>(csvData, classStructure);
        }

        public List<T> ValidateCsvData<T>(IEnumerable<string> csvData, Dictionary<string, Type> classStructure)
        {
            var objects = new List<T>();
            var headers = csvData.First().Split(',');

            foreach (var line in csvData.Skip(1))
            {
                var values = line.Split(',');

                T obj = CreateObject<T>(classStructure);

                for (int i = 0; i < Math.Min(headers.Length, values.Length); i++)
                {
                    var header = headers[i].Trim();
                    var value = values[i];

                    SetProperty(obj, header, value, classStructure[header]);
                }

                objects.Add(obj);
            }

            ValidateObjects(objects);

            return objects;
        }

        private void ValidateObjects<T>(IEnumerable<T> objects)
        {
            foreach (var obj in objects)
            {
                ValidateObject(obj);
            }
        }

        private void ValidateObject<T>(T obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
            {
                Console.WriteLine($"Validation failed for {obj.GetType().Name}. Errors:");

                foreach (var result in validationResults)
                {
                    Console.WriteLine($"- {result.ErrorMessage}");
                }
            }
        }

        private T CreateObject<T>(Dictionary<string, Type> classStructure)
        {
            T obj = Activator.CreateInstance<T>();

            foreach (var property in classStructure.Keys)
            {
                SetProperty(obj, property, null, classStructure[property]);
            }

            return obj;
        }

        private void SetProperty<T>(T obj, string propertyName, string value, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                var convertedValue = Convert.ChangeType(value, propertyType);
                obj.GetType().GetProperty(propertyName)?.SetValue(obj, convertedValue);
            }
            else
            {
                // Handle other types as needed
            }
        }
    }
}
