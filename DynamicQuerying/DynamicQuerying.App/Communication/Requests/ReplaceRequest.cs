using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.App.Communication.Requests
{
    public class ReplaceRequest<T>
    {
        public QueryRequest QueryRequest { get; set; }
        public T Item { get; set; }
    }
}