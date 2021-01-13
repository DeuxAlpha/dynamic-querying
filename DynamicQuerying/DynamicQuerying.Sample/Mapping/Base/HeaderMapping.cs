using System.Collections.Generic;
using CsvHelper.Configuration;

namespace DynamicQuerying.Sample.Mapping.Base
{
    // Only inherits for better understanding of purpose.
    public abstract class HeaderMapping<T> : ClassMap<T>
    {
    }
}