using DynamicQuerying.Sample.Mapping.Base;
using DynamicQuerying.Sample.Models;

namespace DynamicQuerying.Sample.Mapping
{
    public sealed class UserHeaderMapping : HeaderMapping<User>
    {
        public UserHeaderMapping()
        {
            Map(user => user.Id).Name("Id");
            Map(user => user.Email).Name("Email");
            Map(user => user.UserName).Name("UserName");
            Map(user => user.FirstName).Name("FirstName");
            Map(user => user.LastName).Name("LastName");
            Map(user => user.Revenue).Name("Revenue");
            Map(user => user.LoggedIn).Name("LoggedIn");
            Map(user => user.Created).Name("Created");
        }
    }
}