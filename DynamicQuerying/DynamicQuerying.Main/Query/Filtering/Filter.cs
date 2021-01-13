using System;
using DynamicQuerying.Main.Query.Filtering.Enums;

namespace DynamicQuerying.Main.Query.Filtering
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public string Comparison { get; set; } = ComparisonEnum.Equal.ToString("F");
        public string Relation { get; set; } = RelationEnum.And.ToString("F");

        internal ComparisonEnum ComparisonEnum =>
            Comparison.ToLower() == "equals"
                ? ComparisonEnum.Equal
                : Enum.Parse<ComparisonEnum>(Comparison, true);
        internal RelationEnum RelationEnum => Enum.Parse<RelationEnum>(Relation, true);
    }
}