# Dynamic Querying Library for .NET
Welcome to the Dynamic Querying Library for .NET! This library provides dynamic querying functionality for .NET applications by allowing users to filter, sort, paginate, and aggregate data using a single query request.

## Getting Started
To get started with this library, pull the package from [NuGet](https://www.nuget.org/packages/DynamicQuerying.Main/), or clone the repo and add the provided classes to your .NET application. Then, use the QueryService class to perform queries on your data.

```csharp
using(var context = new DatabaseContext())
{
    var query = context.Users;

    var request = new QueryRequest
    {
        Filters = new List<Filter>
        {
            new Filter
            {
                PropertyName = "Email",
                ComparisonEnum = ComparisonEnum.Contains,
                Value = "gmail.com"
            }
        }
    };

    var response = QueryService.GetQueryResponse(query, request);

    foreach (var item in response.Items)
    {
        Console.WriteLine(item);
    }
}
```
### Examples
Sorting
```csharp
var request = new QueryRequest
{
    Sorters = new List<Sorter>
    {
        new Sorter
        {
            PropertyName = "Name",
            Direction = DirectionEnum.Ascending.ToString("G")
        }
    }
};
```
Filtering
```csharp
var request = new QueryRequest
{
    Filters = new List<Filter>
    {
        new Filter
        {
            PropertyName = "Email",
            Comparison = ComparisonEnum.Contains.ToString("G"),
            Value = "gmail.com"
        }
    }
};
```
Aggregating
```csharp
var request = new QueryRequest
{
    Aggregators = new List<Aggregator>
    {
        new Aggregator
        {
            PropertyName = "Salary"
        }
    }
};
```
Distincting
```csharp
var request = new QueryRequest
{
    Distinctors = new List<Distinctor>
    {
        new Distinctor
        {
            PropertyName = "Name"
        }
    }
};
```
### Notes
This library uses IQueryable, which may not be performant with larger datasets. It's recommended to test this library with larger datasets before using it in a production environment.
### Disclaimers
This code is provided as-is, and there is no guarantee that it will work with your specific application or use case. Also, any modifications to the code will be the responsibility of the user. Please thoroughly test the code before using it in a production environment.

### Additional Notes
 
To pass any collection to the QueryService, use the AsQueryable() extension method.

Take care to validate how querying actually works. For example, using
this type of request:  
```json
{
    "Filters": [{
        "PropertyName": "FirstName",
        "Relation": "And",
        "Comparison": "Equal",
        "Value": "Jeremy"
    }, {
        "PropertyName": "LastName",
        "Relation": "Or",
        "Comparison": "Equal",
        "Value": "With"
    }, {
        "PropertyName": "FirstName",
        "Relation": "And",
        "Comparison": "Equal",
        "Value": "Jeremy"
    }, {
        "PropertyName": "LastName",
        "Comparison": "Equal",
        "Value": "Out"
    }]
}
```
Is going to result only in results for Jeremy Out.

However, the following request:  
```json
{
    "Filters": [
        {
            "PropertyName": "LastName",
            "Relation": "OrElse",
            "Comparison": "Equal",
            "Value": "With"
        },
        {
            "PropertyName": "LastName",
            "Relation": "AndAlso",
            "Comparison": "Equal",
            "Value": "Out"
        },
        {
            "PropertyName": "FirstName",
            "Comparison": "Equal",
            "Value": "Jeremy"
        }
    ]
}
```
Is going to result in results for Jeremy With _and_ Jeremy Out.

In the library, the order of applied filters impacts how the results get resolved.
