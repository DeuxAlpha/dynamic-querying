# Notes
 
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
