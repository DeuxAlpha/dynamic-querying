# Notes

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
        "Value": "Flucker"
    }]
}
```
Is going to result only in results for Jeremy Flucker.

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
            "Value": "Flucker"
        },
        {
            "PropertyName": "FirstName",
            "Comparison": "Equal",
            "Value": "Jeremy"
        }
    ]
}
```
Is going to result in results for Jeremy With _and_ Jeremy Flucker.

The library is powerful, but the order of applied filters _does_ matter,
of course. It could be hard to troubleshoot at times.
