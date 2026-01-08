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
        builder.HasKey(e => e.UsersId);
        
        builder.Property(e => e.UsersId).HasColumnName("users_id").HasConversion<byte[]>();
        builder.Property(e => e.NormalizedEmail).HasColumnName("normalized_email").HasMaxLength(45);
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255);
        builder.Property(e => e.RefreshToken).HasColumnName("refresh_token").HasMaxLength(255);

        builder.HasOne(d => d.User)
            .WithOne() // Assuming User doesn't need a back-reference to Credentials in Domain
            .HasForeignKey<UserCredential>(d => d.UsersId)
            .HasConstraintName("fk_user_credentials_users1");
    }
}