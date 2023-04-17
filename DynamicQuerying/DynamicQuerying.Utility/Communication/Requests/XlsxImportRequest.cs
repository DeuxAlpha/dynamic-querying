using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class XlsxImportRequest
    {
        public IFormFile ExcelFile { get; set; }
        public string SheetName { get; set; }
    }
}