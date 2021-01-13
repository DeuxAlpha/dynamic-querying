using System.Linq;
using System.Linq.Expressions;
using DynamicQuerying.Main.Query.Sorting;
using DynamicQuerying.Main.Query.Sorting.Enums;

namespace DynamicQuerying.Main.Expressions
{
    internal static class SorterExpressions
    {
        public static IQueryable<T> OrderByMember<T>(IQueryable<T> source, Sorter sorter)
        {
            var parameter = Expression.Parameter(typeof(T));
            var member = BaseExpressions.GetDotMember(parameter, sorter.PropertyName);
            var lambda = Expression.Lambda(member, parameter);

            var orderMethod = typeof(Queryable)
                .GetMethods()
                .First(method => method.Name == (sorter.SortDirectionEnum == SortDirectionEnum.Ascending
                    ? nameof(Queryable.OrderBy)
                    : nameof(Queryable.OrderByDescending)));
            var genericOrderMethod = orderMethod.MakeGenericMethod(source.ElementType, member.Type);

            var methodCall = Expression.Call(genericOrderMethod, source.Expression, lambda);
            return source.Provider.CreateQuery<T>(methodCall);
        }

        public static IQueryable<T> ThenOrderByMember<T>(IQueryable<T> source, Sorter sorter)
        {
            var parameter = Expression.Parameter(typeof(T));
            var member = BaseExpressions.GetDotMember(parameter, sorter.PropertyName);
            var lambda = Expression.Lambda(member, parameter);

            var orderMethod = typeof(Queryable)
                .GetMethods()
                .First(method => method.Name == (sorter.SortDirectionEnum == SortDirectionEnum.Ascending
                    ? nameof(Queryable.ThenBy)
                    : nameof(Queryable.ThenByDescending)));
            var genericOrderMethod = orderMethod.MakeGenericMethod(source.ElementType, member.Type);

            var methodCall = Expression.Call(genericOrderMethod, source.Expression, lambda);
            return source.Provider.CreateQuery<T>(methodCall);
        }
    }
}