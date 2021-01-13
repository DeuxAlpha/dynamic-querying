using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CsvHelper;
using DynamicQuerying.Sample.Extensions;
using DynamicQuerying.Sample.Mapping.Base;
using Newtonsoft.Json;

namespace DynamicQuerying.Sample.Services
{
    public static class ImportService
    {
        public static IEnumerable<T> ImportDataFromXlsx<T>(
            Stream fileStream,
            string sheetName,
            HeaderMapping<T> headerMapping) where T : new()
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheets.FirstOrDefault(sheet =>
                string.IsNullOrWhiteSpace(sheetName) || // If sheetName is null, just pick the first sheet
                sheet.Name == sheetName);
            if (worksheet == null) throw new NullReferenceException("Worksheet is null.");

            // Assigning column order, just in case
            var columns = new List<HeaderPropertyCombination>();
            var headerRow = worksheet.FirstRowUsed();
            foreach (var cell in headerRow.Cells(1, headerMapping.Length))
            {
                var header = headerMapping.MemberMaps
                    .FirstOrDefault(name => name.GetAssignedName() == cell.Value.ToString());
                if (header == null)
                    throw new NullReferenceException($"Could not find associated header. Value: {cell.Value}");
                columns.Add(new HeaderPropertyCombination
                {
                    Header = header.GetAssignedName(),
                    Property = header.GetOriginalName()
                });
            }

            var currentRow = headerRow.RowBelow();
            var items = new List<T>();
            while (currentRow.CellsUsed().Any())
            {
                var item = new T();
                foreach (var cell in currentRow.CellsUsed()) // Using CellsUsed because we don't want to waste time on empty cells.
                {
                    var columnIndex = cell.WorksheetColumn().ColumnNumber() - 1;
                    if (columnIndex > columns.Count - 1) continue; // There's a chance that there are more columns used than headers we associated with it.
                    item.AssignValue(columns[columnIndex].Property, cell.Value);
                }

                items.Add(item);
                currentRow = currentRow.RowBelow();
            }

            return items;
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

        public static async Task<IEnumerable<T>> ImportDataFromJson<T>(Stream fileStream, HeaderMapping<T> headerMapping)
        {
            using var streamReader = new StreamReader(fileStream);
            var content = await streamReader.ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(content, new JsonSerializerSettings
            {
                ContractResolver = new JsonHeaderContractResolver<T>(headerMapping)
            });

            return data;
        }

        private class HeaderPropertyCombination
        {
            public string Header { get; init; }
            public string Property { get; init; }
        }
    }
}