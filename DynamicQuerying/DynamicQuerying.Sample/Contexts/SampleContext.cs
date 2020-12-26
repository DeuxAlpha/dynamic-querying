using DynamicQuerying.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicQuerying.Sample.Contexts
{
    public class SampleContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Don't use plaintext connection strings in code, kids.
            optionsBuilder.UseSqlServer("Server=localhost,20302;Database=sample-db;User Id=sa;Password=Your_password123;");
        }
    }
}