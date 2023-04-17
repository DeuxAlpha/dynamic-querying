using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DynamicQuerying.Sample.Extensions;
using Newtonsoft.Json;

namespace DynamicQuerying.Sample.Services
{
    public static class ExportService
    {
        public static async Task<byte[]> ExportDataToXlsx<T>(IEnumerable<T> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet();
            var headerRow = worksheet.FirstRow();
            var properties = typeof(T).GetProperties();
            for (var headerIndex = 0; headerIndex < properties.Length; headerIndex++)
            {
                headerRow.Cell(headerIndex + 1).SetValue(properties[headerIndex].Name);
            }

            foreach (var row in data)
            {
                var nextRow = worksheet.LastRowUsed().RowBelow();
                for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var cellValue = new XLCellValue();
                    cellValue.AssignValue(properties[columnIndex].Name, properties[columnIndex].GetValue(row, null));
                    nextRow.Cell(columnIndex + 1).SetValue(cellValue);
                }
            }

            await using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ExportDataToCsv<T>(IEnumerable<T> data, string delimiter)
        {
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
            csvConfig.Delimiter = delimiter;
            await using var csvWriter = new CsvWriter(streamWriter, csvConfig);
            csvWriter.WriteHeader<T>();
            await csvWriter.WriteRecordsAsync(data);

            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ExportDataToJson<T>(
            IEnumerable<T> data,
            bool indented)
        {
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream);
            var serialized = JsonConvert.SerializeObject(
                data,
                indented ? Formatting.Indented : Formatting.None);

            await streamWriter.WriteAsync(serialized);
            await streamWriter.FlushAsync();

            return memoryStream.ToArray();
        }
    }
}