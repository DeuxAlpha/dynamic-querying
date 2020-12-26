using DynamicQuerying.Main.Query.Filtering.Enums;

namespace DynamicQuerying.Main.Extensions
{
    internal static class ComparisonExtensions
    {
        public static bool IsStringComparison(this Comparison comparison)
        {
            return comparison == Comparison.Contains ||
                   comparison == Comparison.StartsWith ||
                   comparison == Comparison.EndsWith;
        }
    }
}