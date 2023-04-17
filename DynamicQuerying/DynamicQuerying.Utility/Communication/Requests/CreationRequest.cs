using System.Collections.Generic;

namespace DynamicQuerying.Sample.Communication.Requests
{
    public class CreationRequest<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}