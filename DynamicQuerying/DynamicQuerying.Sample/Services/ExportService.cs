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
    public static class ExportService
    {
        public static async Task<byte[]> ExportDataToXlsx<T>(IEnumerable<T> data, HeaderMapping<T> mapping)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet();
            var headerRow = worksheet.FirstRow();
            for (var headerIndex = 0; headerIndex < mapping.MemberMaps.Count; headerIndex++)
            {
                headerRow.Cell(headerIndex + 1).SetValue(mapping.MemberMaps[headerIndex].GetAssignedName());
            }

            foreach (var row in data)
            {
                var nextRow = worksheet.LastRowUsed().RowBelow();
                var properties = row.GetType().GetProperties();
                for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    nextRow.Cell(columnIndex + 1).SetValue(properties[columnIndex].GetValue(row, null));
                }
            }

            await using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ExportDataToCsv<T>(IEnumerable<T> data, HeaderMapping<T> mapping, string delimiter)
        {
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture)
            {
                Configuration =
                {
                    Delimiter = delimiter
                }
            };
            csvWriter.Configuration.RegisterClassMap(mapping);
            await csvWriter.WriteRecordsAsync(data);

            return memoryStream.ToArray();
        }

        public static async Task<byte[]> ExportDataToJson<T>(
            IEnumerable<T> data,
            HeaderMapping<T> mapping,
            bool indented)
        {
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream);
            var serialized = JsonConvert.SerializeObject(
                data,
                indented ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings
                {
                    ContractResolver = new JsonHeaderContractResolver<T>(mapping)
                });

            await streamWriter.WriteAsync(serialized);
            await streamWriter.FlushAsync();

            return memoryStream.ToArray();
        }
    }
}