using DynamicQuerying.Main.Query.Filtering.Enums;

namespace DynamicQuerying.Main.Extensions
{
    internal static class ComparisonExtensions
    {
        public static bool IsStringComparison(this ComparisonEnum comparisonEnum)
        {
            return comparisonEnum == ComparisonEnum.Contains ||
                   comparisonEnum == ComparisonEnum.StartsWith ||
                   comparisonEnum == ComparisonEnum.EndsWith;
        }
    }
}