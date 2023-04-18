using System.Linq;
using DynamicQuerying.Main.Query.Aggregate;
using DynamicQuerying.Main.Query.Distinct;
using DynamicQuerying.Main.Query.Filtering;
using DynamicQuerying.Main.Query.Filtering.Enums;
using DynamicQuerying.Main.Query.Models;
using DynamicQuerying.Main.Query.Services;
using DynamicQuerying.Main.Query.Sorting;
using DynamicQuerying.Main.Query.Sorting.Enums;
using NUnit.Framework;

namespace DynamicQuerying.Tests
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public void QueryCollection_ValidParameters_QueriesCollection1()
        {
            var usersCollection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out"
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young"
                }
            };

            var query = new QueryRequest
            {
                Filters = new[]
                {
                    new Filter
                    {
                        PropertyName = "FirstName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Jeremy",
                        Relation = RelationEnum.And.ToString("G"),
                    },
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "With",
                        Relation = RelationEnum.Or.ToString("G")
                    },
                    new Filter
                    {
                        PropertyName = "FirstName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Jeremy",
                        Relation = RelationEnum.And.ToString("G"),
                    },
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Out"
                    }
                }
            };

            var result = QueryService.GetQueryResponse(usersCollection.AsQueryable(), query);

            Assert.AreEqual(1, result.MaxItemCount);
            Assert.AreEqual(1, result.ItemCount);
            Assert.That(result.Items.First().FirstName, Is.EqualTo("Jeremy"));
            Assert.That(result.Items.First().LastName, Is.EqualTo("Out"));
        }
        
        [Test]
        public void QueryCollection_ValidParameters_QueriesCollection2()
        {
            var usersCollection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out"
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young"
                }
            };

            var query = new QueryRequest
            {
                Filters = new[]
                {
                    
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "With",
                        Relation = RelationEnum.AndAlso.ToString("G")
                    },
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Out",
                        Relation = RelationEnum.OrElse.ToString("G")
                    },
                    new Filter
                    {
                        PropertyName = "FirstName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Jeremy"
                    }
                }
            };

            var result = QueryService.GetQueryResponse(usersCollection.AsQueryable(), query);

            Assert.AreEqual(2, result.MaxItemCount);
            Assert.AreEqual(2, result.ItemCount);
            Assert.That(result.Items.First().FirstName, Is.EqualTo("Jeremy"));
            Assert.That(result.Items.First().LastName, Is.EqualTo("With"));
            Assert.That(result.Items.ElementAt(1).FirstName, Is.EqualTo("Jeremy"));
            Assert.That(result.Items.ElementAt(1).LastName, Is.EqualTo("Out"));
        }
        
        [Test]
        public void QueryCollection_Aggregators_AggregatesCollection()
        {
            var usersCollection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                    Revenue = 100
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out",
                    Revenue = 200
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young",
                    Revenue = 300
                }
            };

            var query = new QueryRequest
            {
                Aggregators = new[]
                {
                    new Aggregator
                    {
                        PropertyName = "Revenue",
                    }
                }
            };

            var result = QueryService.GetQueryResponse(usersCollection.AsQueryable(), query);

            Assert.AreEqual(1, result.Aggregations.Count());
            Assert.AreEqual("Revenue", result.Aggregations.First().PropertyName);
            Assert.AreEqual(600, result.Aggregations.First().Sum);
            Assert.AreEqual(200, result.Aggregations.First().Average);
            Assert.AreEqual(100, result.Aggregations.First().Min);
            Assert.AreEqual(300, result.Aggregations.First().Max);
        }
        
        [Test]
        public void QueryCollection_AggregatorsAndFilters_AggregatesAndFiltersCollection()
        {
            var usersCollection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                    Revenue = 100
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out",
                    Revenue = 200
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young",
                    Revenue = 300
                }
            };

            var query = new QueryRequest
            {
                Filters = new[]
                {
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "With",
                        Relation = RelationEnum.AndAlso.ToString("G")
                    },
                    new Filter
                    {
                        PropertyName = "LastName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Out",
                        Relation = RelationEnum.OrElse.ToString("G")
                    },
                    new Filter
                    {
                        PropertyName = "FirstName",
                        Comparison = ComparisonEnum.Equal.ToString("G"),
                        Value = "Jeremy"
                    }
                },
                Aggregators = new[]
                {
                    new Aggregator
                    {
                        PropertyName = "Revenue",
                    }
                }
            };

            var result = QueryService.GetQueryResponse(usersCollection.AsQueryable(), query);

            Assert.AreEqual(1, result.Aggregations.Count());
            Assert.AreEqual("Revenue", result.Aggregations.First().PropertyName);
            Assert.AreEqual(300, result.Aggregations.First().Sum);
            Assert.AreEqual(150, result.Aggregations.First().Average);
            Assert.AreEqual(100, result.Aggregations.First().Min);
            Assert.AreEqual(200, result.Aggregations.First().Max);
        }

        [Test]
        public void QueryCollection_Distinctors_ValidDistinctValues()
        {
            var collection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                    Revenue = 100
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out",
                    Revenue = 200
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young",
                    Revenue = 300
                }
            };
            
            var query = new QueryRequest
            {
                Distinctions = new[]
                {
                    new Distinctor
                    {
                        PropertyName = "FirstName"
                    }
                }
            };
            
            var result = QueryService.GetQueryResponse(collection.AsQueryable(), query);
            
            Assert.AreEqual(1, result.Distinctions.Count());
            Assert.AreEqual("FirstName", result.Distinctions.First().PropertyName);
            Assert.AreEqual(2, result.Distinctions.First().Values.Count());
            Assert.AreEqual("Jeremy", result.Distinctions.First().Values.First());
            Assert.AreEqual("Evan", result.Distinctions.First().Values.ElementAt(1));
        }
        
        [Test]
        public void QueryCollection_Sorters_ValidSortValues()
        {
            var collection = new[]
            {
                new
                {
                    FirstName = "Jeremy",
                    LastName = "With",
                    Revenue = 100
                },
                new
                {
                    FirstName = "Jeremy",
                    LastName = "Out",
                    Revenue = 200
                },
                new
                {
                    FirstName = "Evan",
                    LastName = "Young",
                    Revenue = 300
                }
            };
            
            var query = new QueryRequest
            {
                Sorters = new[]
                {
                    new Sorter
                    {
                        PropertyName = "FirstName",
                        SortDirection = SortDirectionEnum.Ascending.ToString("G")
                    },
                    new Sorter
                    {
                        PropertyName = "Revenue",
                        SortDirection = SortDirectionEnum.Descending.ToString("G")
                    }
                }
            };
            
            var result = QueryService.GetQueryResponse(collection.AsQueryable(), query);
            
            Assert.AreEqual(3, result.Items.Count());
            Assert.AreEqual("Evan", result.Items.First().FirstName);
            Assert.AreEqual("Out", result.Items.ElementAt(1).LastName);
            Assert.AreEqual("With", result.Items.ElementAt(2).LastName);
        }
    }
}