using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(45);
            builder.Property(e => e.Email).HasColumnName("email").HasMaxLength(45);
            builder.Property(e => e.EmailVerified).HasColumnName("email_verified").HasColumnType("tinyint");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(e => e.Email).IsUnique().HasDatabaseName("email_UNIQUE");
        }
    }
}