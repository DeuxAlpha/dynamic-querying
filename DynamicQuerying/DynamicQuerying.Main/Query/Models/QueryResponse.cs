using System.Collections;
using System.Collections.Generic;
using DynamicQuerying.Main.Query.Aggregate;
using DynamicQuerying.Main.Query.Distinct;

namespace DynamicQuerying.Main.Query.Models
{
    public class QueryResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int ItemCount { get; set; }
        public int MaxItemCount { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public int StartItemCount { get; set; }
        public int EndItemCount { get; set; }
        public IEnumerable<Aggregation> Aggregations { get; set; } = new List<Aggregation>();
        public IEnumerable<Distinction> Distinctions { get; set; } = new List<Distinction>();

        
    }
}