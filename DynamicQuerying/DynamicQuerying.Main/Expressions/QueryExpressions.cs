using System;
using System.Linq;
using System.Linq.Expressions;
using DynamicQuerying.Main.Extensions;
using DynamicQuerying.Main.Query.Aggregate;
using DynamicQuerying.Main.Query.Distinct;
using DynamicQuerying.Main.Query.Models;

namespace DynamicQuerying.Main.Expressions
{
    internal static class QueryExpressions
    {
        public static QueryResponse<T> PaginateRequest<T>(IQueryable<T> source, QueryRequest queryRequest)
        {
            var pageIndex = queryRequest.Page - 1;
            var count = source.Count();
            var items = source
                .Skip(queryRequest.Items * pageIndex.ThisIfLess(0))
                .Take(queryRequest.Items)
                .ToList();
            var maxItemInfo = MaxItemInfo.Build(count, queryRequest.Items);
            var startItemCount = queryRequest.Items * (queryRequest.Page.ThisIfLess(1) - 1) + 1;
            // For page = 1, start would be 1. For page 2, start would be items + 1. etc.

            return new QueryResponse<T>
            {
                Items = items,
                ItemCount = items.Count,
                Page = queryRequest.Page.ThisIfLess(1),
                StartItemCount = startItemCount,
                EndItemCount = startItemCount + items.Count - 1,
                MaxPage = maxItemInfo.Page,
                MaxItemCount = maxItemInfo.Items
            };
        }

        private class MaxItemInfo
        {
            public int Items { get; set; }
            public int Page { get; set; }

            public MaxItemInfo(int items, int page)
            {
                Items = items;
                Page = page;
            }

            public static MaxItemInfo Build(int maxItemCount, int requestedItems)
            {
                if (requestedItems <= 0) requestedItems = 1; // Avoid divisions by zero exceptions
                var divided = maxItemCount / requestedItems;
                var remainder = maxItemCount % requestedItems;
                return remainder == 0
                    ? new MaxItemInfo(maxItemCount, divided) // E.g. 30/15=2, rem. 0 --> MaxPage should be 2.
                    : new MaxItemInfo(maxItemCount, divided + 1); // E.g. 25/15=1, rem. 10 --> MaxPage should be 2.
            }
        }

        public static QueryResponse<T> FinishRequest<T>(
            IGrouping<int, T> source,
            QueryRequest queryRequest,
            QueryResponse<T> queryResponse)
        {
            var parameter = Expression.Parameter(typeof(T));
            if (queryRequest.Aggregators != null && queryRequest.Aggregators.Any())
            {
                queryResponse.Aggregations = from aggregator in queryRequest.Aggregators
                    let body = BaseExpressions.GetDotMember(parameter, aggregator.PropertyName)
                    let converted = Expression.Convert(body, typeof(decimal?))
                    let lambda = Expression.Lambda<Func<T, decimal?>>(converted, parameter).Compile()
                    select new Aggregation
                    {
                        PropertyName = aggregator.PropertyName,
                        Sum = source.Sum(lambda),
                        Max = source.Max(lambda),
                        Min = source.Min(lambda),
                        Average = source.Average(lambda)
                    };
            }

            if (queryRequest.Distinctions != null && queryRequest.Distinctions.Any())
            {
                queryResponse.Distinctions = from distinctor in queryRequest.Distinctions
                    let body = BaseExpressions.GetDotMember(parameter, distinctor.PropertyName)
                    let converted = Expression.Convert(body, typeof(object))
                    let lambda = Expression.Lambda<Func<T, object>>(converted, parameter).Compile()
                    select new Distinction
                    {
                        PropertyName = distinctor.PropertyName,
                        Values = source.Select(lambda).Distinct().Where(value => value != null).ToList()
                    };
            }

            return queryResponse;
        }
    }
}