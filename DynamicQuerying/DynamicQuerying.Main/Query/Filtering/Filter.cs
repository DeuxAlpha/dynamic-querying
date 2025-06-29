using DynamicQuerying.Main.Extensions;
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
                : EnumEx.ToEnum<ComparisonEnum>(Comparison);
        internal RelationEnum RelationEnum => EnumEx.ToEnum<RelationEnum>(Relation);
    }
}