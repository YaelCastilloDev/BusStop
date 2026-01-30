using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations;

public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
{
    public void Configure(EntityTypeBuilder<UserCredential> builder)
    {
        builder.ToTable("user_credentials");
        builder.HasKey(e => e.Id); // Use Id

        builder.Property(e => e.Id)
               .HasColumnName("users_id"); // Your SQL column name

        builder.Property(e => e.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(45);
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
        builder.Property(e => e.RefreshToken).HasColumnName("refresh_token").HasMaxLength(255);

        builder.HasOne(d => d.User)
            .WithOne()
            .HasForeignKey<UserCredential>(d => d.Id) // Use Id
            .HasConstraintName("fk_user_credentials_users1");
    }
}