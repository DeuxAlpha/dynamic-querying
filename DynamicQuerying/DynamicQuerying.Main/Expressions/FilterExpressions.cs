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
                expression = previousFilter.RelationEnum switch
                {
                    RelationEnum.And => Expression.And(expression, GetComparingExpression(parameter, currentFilter)),
                    RelationEnum.AndAlso => Expression.AndAlso(expression,
                        GetComparingExpression(parameter, currentFilter)),
                    RelationEnum.Or => Expression.Or(expression, GetComparingExpression(parameter, currentFilter)),
                    RelationEnum.OrElse => Expression.OrElse(expression, GetComparingExpression(parameter, currentFilter)),
                    RelationEnum.XOr => Expression.ExclusiveOr(expression,
                        GetComparingExpression(parameter, currentFilter)),
                    _ => throw new ArgumentOutOfRangeException(nameof(previousFilter.RelationEnum), previousFilter.RelationEnum,
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
                : filter.ComparisonEnum.IsStringComparison()
                    ? Expression.Constant(filter.Value.ToString())
                    : BaseExpressions.ConvertValue(property, filter.Value);
            return BuildComparingExpression(property, comparingValue, filter.ComparisonEnum);
        }

        private static Expression BuildComparingExpression(Expression left, Expression right, ComparisonEnum comparisonEnum)
        {
            return comparisonEnum switch
            {
                ComparisonEnum.Equal => Expression.Equal(left, right),
                ComparisonEnum.LessThan => Expression.LessThan(left, right),
                ComparisonEnum.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
                ComparisonEnum.GreaterThan => Expression.GreaterThan(left, right),
                ComparisonEnum.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
                ComparisonEnum.NotEqual => Expression.NotEqual(left, right),
                ComparisonEnum.Contains => Expression.Call(left, ContainsMethod, right),
                ComparisonEnum.StartsWith => Expression.Call(left, StartsWithMethod, right),
                ComparisonEnum.EndsWith => Expression.Call(left, EndsWithMethod, right),
                ComparisonEnum.NotNull => Expression.NotEqual(left, Expression.Constant(null)),
                ComparisonEnum.Null => Expression.Equal(left, Expression.Constant(null)),
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonEnum), comparisonEnum, null)
            };
        }
    }
}