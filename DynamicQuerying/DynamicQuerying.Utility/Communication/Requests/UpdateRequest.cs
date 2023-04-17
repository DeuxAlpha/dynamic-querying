using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class UpdateRequest<T>
    {
        public QueryRequest QueryRequest { get; set; }
        public T Item { get; set; }
    }
}