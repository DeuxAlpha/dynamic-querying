namespace DynamicQuerying.Main.Query.Aggregate
{
    public class Aggregation
    {
        public string PropertyName { get; set; }
        public decimal? Sum { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? Average { get; set; }
    }
}