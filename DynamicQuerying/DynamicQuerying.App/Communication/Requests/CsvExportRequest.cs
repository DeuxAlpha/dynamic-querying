using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.App.Communication.Requests
{
    public class CsvExportRequest
    {
        public QueryRequest QueryRequest { get; set; } = new();
        public string Delimiter { get; set; } = ",";
    }
}