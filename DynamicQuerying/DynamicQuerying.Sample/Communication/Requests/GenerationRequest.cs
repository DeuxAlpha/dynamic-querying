using System.Collections.Generic;
using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.Sample.Communication.Requests
{
    // The way the GenerationRequest is used is as follows:
    // The QueryRequest first queries all items.
    // Then, for each item that was selected, the properties of each item in the item collection get applied to a new model.
    // However, also for each item, the values based on the CopiedProperties collection get assigned to the new models.
    // In other words, copying over the values of the original item.
    public class GenerationRequest<T>
    {
        public QueryRequest QueryRequest { get; set; }
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<string> CopiedProperties { get; set; }
    }
}