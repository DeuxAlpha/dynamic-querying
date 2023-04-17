using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class CsvImportRequest
    {
        public IFormFile CsvFile { get; set; }
        public string Delimiter { get; set; } = ",";
    }
}