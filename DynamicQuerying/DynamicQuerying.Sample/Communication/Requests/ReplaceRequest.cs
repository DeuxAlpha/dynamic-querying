using System.Collections.Generic;
using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class ReplaceRequest<T>
    {
        public QueryRequest QueryRequest { get; set; }
        public T Item { get; set; }
    }
}