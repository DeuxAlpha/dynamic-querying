using DynamicQuerying.Main.Query.Filtering.Enums;

namespace DynamicQuerying.Main.Query.Filtering
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public Comparison Comparison { get; set; } = Comparison.Equal;
        public Relation Relation { get; set; } = Relation.And;
    }
}