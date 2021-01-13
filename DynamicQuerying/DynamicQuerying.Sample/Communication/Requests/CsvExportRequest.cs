using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class DelimitedExportRequest
    {
        public QueryRequest QueryRequest { get; set; } = new();
        public string Delimiter { get; set; } = ",";
    }
}