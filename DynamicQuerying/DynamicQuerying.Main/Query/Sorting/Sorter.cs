using DynamicQuerying.Main.Extensions;
using DynamicQuerying.Main.Query.Sorting.Enums;

namespace DynamicQuerying.Main.Query.Sorting
{
    public class Sorter
    {
        public string PropertyName { get; set; }
        public string SortDirection { get; set; } = SortDirectionEnum.Ascending.ToString("F");
        internal SortDirectionEnum SortDirectionEnum => EnumEx.ToEnum<SortDirectionEnum>(SortDirection);
    }
}