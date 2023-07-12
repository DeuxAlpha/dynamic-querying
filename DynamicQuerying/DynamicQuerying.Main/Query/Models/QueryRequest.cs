using System.Collections;
using System.Collections.Generic;
using DynamicQuerying.Main.Query.Aggregate;
using DynamicQuerying.Main.Query.Distinct;
using DynamicQuerying.Main.Query.Filtering;
using DynamicQuerying.Main.Query.Sorting;

namespace DynamicQuerying.Main.Query.Models
{
    // TODO: Provide Request DTO to convert string to Enums (e.g. Contains to 7)
    public class QueryRequest
    {
        public IEnumerable<Filter> Filters { get; set; } = new List<Filter>();
        public IEnumerable<Sorter> Sorters { get; set; } = new List<Sorter>();
        public IEnumerable<Aggregator> Aggregators { get; set; } = new List<Aggregator>();
        public IEnumerable<Distinctor> Distinctors { get; set; } = new List<Distinctor>();
        public int Page { get; set; } = 1;
        public int Items { get; set; } = 30;
    }
}