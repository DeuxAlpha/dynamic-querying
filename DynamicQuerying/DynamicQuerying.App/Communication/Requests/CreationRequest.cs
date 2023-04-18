namespace DynamicQuerying.App.Communication.Requests
{
    public class CreationRequest<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}