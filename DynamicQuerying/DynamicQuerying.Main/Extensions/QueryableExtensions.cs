using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DynamicQuerying.Main.Expressions;
using DynamicQuerying.Main.Query.Filtering;
using DynamicQuerying.Main.Query.Sorting;

namespace DynamicQuerying.Main.Extensions
{
    internal static class QueryableExtensions
    {
        public static IQueryable<T> FilterBy<T>(this IQueryable<T> source, IEnumerable<Filter> filters)
        {
            return FilterExpressions.FilterByMembers(source, filters);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<Sorter> sorters)
        {
            if (sorters == null) return source;
            var sorterList = sorters.ToList();
            if (sorterList.Empty()) return source;
            for (var index = 0; index < sorterList.Count; index++)
            {
                var sorter = sorterList[index];
                source = index == 0
                    ? SorterExpressions.OrderByMember(source, sorter)
                    : SorterExpressions.ThenOrderByMember(source, sorter);
            }

            return source;
        }
    }
}