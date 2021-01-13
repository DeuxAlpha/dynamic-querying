using System;

namespace DynamicQuerying.Sample.Models.Dtos.Users
{
    public class UserCreationDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Revenue { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime Created { get; set; } // Handle auto-setting

        public User Map()
        {
            return new()
            {
                Id = Id,
                Email = Email,
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                Revenue = Revenue,
                LoggedIn = LoggedIn,
                Created = Created
            };
        }
    }
}