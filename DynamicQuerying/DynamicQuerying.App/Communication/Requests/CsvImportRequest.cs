namespace DynamicQuerying.App.Communication.Requests
{
    public class CsvImportRequest
    {
        public IFormFile CsvFile { get; set; }
        public string Delimiter { get; set; } = ",";
    }
}