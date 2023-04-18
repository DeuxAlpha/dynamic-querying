namespace DynamicQuerying.App.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Revenue { get; set; }
        public bool? LoggedIn { get; set; }
        public DateTime? Created { get; set; }
    }
}