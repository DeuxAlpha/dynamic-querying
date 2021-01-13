using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CsvHelper;
using DynamicQuerying.Sample.Mapping.Base;

namespace DynamicQuerying.Sample.Services
{
    public static class ImportService
    {
        public static async Task<IEnumerable<T>> ImportDataFromXlsx<T>(
            Stream fileStream,
            string sheetName,
            HeaderMapping<T> headerMapping)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheets.FirstOrDefault(sheet => sheet.Name == sheetName);
            if (worksheet == null) throw new NullReferenceException("Worksheet is null.");
            var columns = new List<string>();
            var headerRow = worksheet.FirstRow();
            var properties = headerRow.GetType().GetProperties();
            throw new NotImplementedException();
        }

        public static IEnumerable<T> ImportDataFromCsv<T>(
            Stream fileStream,
            HeaderMapping<T> headerMapping,
            string delimiter)
        {
            using var streamReader = new StreamReader(fileStream);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture)
            {
                Configuration = {Delimiter = delimiter}
            };
            csvReader.Configuration.RegisterClassMap(headerMapping);
            var data = csvReader.GetRecords<T>().ToList();

            return data;
        }
    }
}