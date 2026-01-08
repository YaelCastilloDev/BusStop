using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(40).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName("name_UNIQUE");

        builder.HasMany(p => p.Users)
            .WithMany(p => p.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "roles_has_users",
                j => j.HasOne<User>()
                      .WithMany()
                      .HasForeignKey("users_id")
                      .HasConstraintName("fk_roles_has_users_users1"), // Crucial for BINARY(16)
                j => j.HasOne<Role>()
                      .WithMany()
                      .HasForeignKey("roles_id")
                      .HasConstraintName("fk_roles_has_users_roles1"),
                j =>
                {
                    j.HasKey("roles_id", "users_id");
                    j.ToTable("roles_has_users");

                    j.Property<Guid>("users_id")
                .HasColumnName("users_id")
                .HasConversion<byte[]>(); // This tells EF to store the Guid as bytes in the join tabl
                });
    }
}