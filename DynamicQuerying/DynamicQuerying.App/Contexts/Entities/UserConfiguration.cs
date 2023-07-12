using DynamicQuerying.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicQuerying.App.Contexts.Entities
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("ID").IsRequired().ValueGeneratedNever();

            builder.Property(u => u.UserName).HasMaxLength(255).IsRequired();

            builder.Property(u => u.FirstName).HasMaxLength(255);

            builder.Property(u => u.LastName).HasMaxLength(255);

            builder.Property(u => u.Revenue).IsRequired();

            builder.Property(u => u.LoggedIn).HasDefaultValueSql("0").IsRequired();

            builder.Property(u => u.Created).IsRequired();
        }
    }
}