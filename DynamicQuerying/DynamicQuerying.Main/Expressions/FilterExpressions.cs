using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DynamicQuerying.Main.Extensions;
using DynamicQuerying.Main.Query.Filtering;
using DynamicQuerying.Main.Query.Filtering.Enums;

namespace DynamicQuerying.Main.Expressions
{
    internal static class FilterExpressions
    {
        private static readonly MethodInfo ContainsMethod =
            typeof(string).GetMethod(nameof(string.Contains), new[] {typeof(string)});

        private static readonly MethodInfo StartsWithMethod =
            typeof(string).GetMethod(nameof(string.StartsWith), new[] {typeof(string)});

        private static readonly MethodInfo EndsWithMethod =
            typeof(string).GetMethod(nameof(string.EndsWith), new[] {typeof(string)});

        private static readonly MethodInfo WhereMethod =
            typeof(Queryable).GetMethods().First(method => method.Name == nameof(Queryable.Where));

        public static IQueryable<T> FilterByMembers<T>(IQueryable<T> source, IEnumerable<Filter> filters)
        {
            if (filters == null) return source;
            var filterList = filters.ToList();
            if (filterList.Empty()) return source;
            var parameter = Expression.Parameter(typeof(T));
            var expression = GetComparingExpression(parameter, filterList[0]);

            for (var index = 1; index < filterList.Count; index++)
            {
                var currentFilter = filterList[index];
                var previousFilter = filterList[index - 1];
                expression = previousFilter.Relation switch
                {
                    Relation.And => Expression.And(expression, GetComparingExpression(parameter, currentFilter)),
                    Relation.AndAlso => Expression.AndAlso(expression,
                        GetComparingExpression(parameter, currentFilter)),
                    Relation.Or => Expression.Or(expression, GetComparingExpression(parameter, currentFilter)),
                    Relation.OrElse => Expression.OrElse(expression, GetComparingExpression(parameter, currentFilter)),
                    Relation.XOr => Expression.ExclusiveOr(expression,
                        GetComparingExpression(parameter, currentFilter)),
                    _ => throw new ArgumentOutOfRangeException(nameof(previousFilter.Relation), previousFilter.Relation,
                        null)
                };
            }

            var whereGenericMethod = WhereMethod.MakeGenericMethod(source.ElementType);

            var lambda = Expression.Lambda(expression, parameter);
            var methodCall = Expression.Call(whereGenericMethod, source.Expression, lambda);
            return source.Provider.CreateQuery<T>(methodCall);
        }

        private static Expression GetComparingExpression(Expression left, Filter filter)
        {
            var property = BaseExpressions.GetDotMember(left, filter.PropertyName);
            var comparingValue = filter.Value == null
                ? Expression.Constant(null)
                : filter.Comparison.IsStringComparison()
                    ? Expression.Constant(filter.Value.ToString())
                    : BaseExpressions.ConvertValue(property, filter.Value);
            return BuildComparingExpression(property, comparingValue, filter.Comparison);
        }

        private static Expression BuildComparingExpression(Expression left, Expression right, Comparison comparison)
        {
            return comparison switch
            {
                Comparison.Equal => Expression.Equal(left, right),
                Comparison.LessThan => Expression.LessThan(left, right),
                Comparison.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
                Comparison.GreaterThan => Expression.GreaterThan(left, right),
                Comparison.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
                Comparison.NotEqual => Expression.NotEqual(left, right),
                Comparison.Contains => Expression.Call(left, ContainsMethod, right),
                Comparison.StartsWith => Expression.Call(left, StartsWithMethod, right),
                Comparison.EndsWith => Expression.Call(left, EndsWithMethod, right),
                Comparison.HasValue => Expression.NotEqual(left, Expression.Constant(null)),
                Comparison.HasNoValue => Expression.Equal(left, Expression.Constant(null)),
                _ => throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null)
            };
        }
    }
}