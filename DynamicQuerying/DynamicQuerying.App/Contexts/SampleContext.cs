using DynamicQuerying.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicQuerying.App.Contexts
{
    public class SampleContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public SampleContext(){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Don't use plaintext connection strings in code, kids.
            optionsBuilder.UseSqlServer("Server=localhost,20342;User Id=sa;Password=Your_password123;TrustServerCertificate=true;");
        }
    }
}