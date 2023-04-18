namespace DynamicQuerying.App.Communication.Requests
{
    public class XlsxImportRequest
    {
        public IFormFile ExcelFile { get; set; }
        public string SheetName { get; set; }
    }
}