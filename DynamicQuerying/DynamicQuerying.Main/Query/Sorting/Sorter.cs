using System;
using DynamicQuerying.Main.Query.Sorting.Enums;

namespace DynamicQuerying.Main.Query.Sorting
{
    public class Sorter
    {
        public string PropertyName { get; set; }
        public string SortDirection { get; set; } = SortDirectionEnum.Ascending.ToString("F");
        internal SortDirectionEnum SortDirectionEnum => Enum.Parse<SortDirectionEnum>(SortDirection, true);
    }
}