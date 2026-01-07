using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    // CHANGE: Use 'User', not 'AppUser'
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("users");

            // 1. Primary Key Mapping
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id").HasColumnType("BINARY(16)");

            // 2. Identity Property Mappings
            builder.Property(u => u.UserName)
                .HasColumnName("name")
                .HasMaxLength(45)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(45)
                .IsRequired();

            // NEW: Matches SQL 'NormalizedEmail VARCHAR(45) NOT NULL'
            builder.Property(u => u.NormalizedEmail)
                .HasColumnName("NormalizedEmail")
                .HasMaxLength(45)
                .IsRequired();

            // NEW: Matches SQL 'password_hash VARCHAR(255) NOT NULL'
            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(255)
                .IsRequired();

            // 3. Custom Property Mappings
            builder.Property(u => u.GoogleId)
                .HasColumnName("google_id")
                .HasMaxLength(255); // Nullable by default in EF if property is string?

            builder.Property(u => u.RoleId).HasColumnName("roles_id");

            // 4. Relationships
            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_users_roles")
                .OnDelete(DeleteBehavior.NoAction);

            // 5. NEW: Unique Indexes from SQL
            builder.HasIndex(u => u.GoogleId).IsUnique().HasDatabaseName("google_id_UNIQUE");
            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("email_UNIQUE");
            builder.HasIndex(u => u.NormalizedEmail).IsUnique().HasDatabaseName("NormalizedEmail_UNIQUE");

            // 6. Ignored Identity Columns (Not in your SQL)
            builder.Ignore(u => u.NormalizedUserName);
            builder.Ignore(u => u.EmailConfirmed);
            builder.Ignore(u => u.SecurityStamp);
            builder.Ignore(u => u.ConcurrencyStamp);
            builder.Ignore(u => u.PhoneNumber);
            builder.Ignore(u => u.PhoneNumberConfirmed);
            builder.Ignore(u => u.TwoFactorEnabled);
            builder.Ignore(u => u.LockoutEnd);
            builder.Ignore(u => u.LockoutEnabled);
            builder.Ignore(u => u.AccessFailedCount);
        }
    }
}