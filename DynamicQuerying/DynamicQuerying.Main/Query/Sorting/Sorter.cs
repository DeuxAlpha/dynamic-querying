using DynamicQuerying.Main.Query.Sorting.Enums;

namespace DynamicQuerying.Main.Query.Sorting
{
    public class Sorter
    {
        public string PropertyName { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}