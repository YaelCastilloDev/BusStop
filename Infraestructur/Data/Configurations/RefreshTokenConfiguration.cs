using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(e => e.Token);

        builder.Property(e => e.Token)
            .HasColumnName("Token")
            .HasMaxLength(255)
            .HasComment("The refresh token string itself");

        builder.Property(e => e.UserId)
            .HasColumnName("UserId")
            .HasConversion<byte[]>() // Conversion for BINARY(16)
            .HasComment("The foreign key to the users table");

        builder.Property(e => e.ExpiresOn)
            .HasColumnName("expires_on")
            .HasColumnType("datetime")
            .HasComment("When the token expires");

        builder.Property(e => e.CreatedOn)
            .HasColumnName("created_on")
            .HasColumnType("datetime")
            .HasComment("When the token was created");

        builder.Property(e => e.RevokedOn)
            .HasColumnName("revoked_on")
            .HasColumnType("datetime")
            .HasComment("When the token was revoked (if it was)");

        // Unidirectional Relationship: User does not need a collection of RefreshTokens
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_refresh_tokens_users");
    }
}