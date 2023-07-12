using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.App.Communication.Requests
{
    public class JsonExportRequest
    {
        public QueryRequest QueryRequest { get; set; }
        public bool Indented { get; set; }
    }
}