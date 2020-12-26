using System.Collections.Generic;

namespace DynamicQuerying.Main.Query.Distinct
{
    public class Distinction
    {
        public string PropertyName { get; set; }
        public IEnumerable<object> Values { get; set; }
    }
}