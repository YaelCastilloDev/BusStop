using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Table mapping
            builder.ToTable("roles");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Property mapping and constraints
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(40)
                .IsRequired();

            // Unique Index
            builder.HasIndex(e => e.Name).IsUnique();

            // Relationships (if any)
            // The one-to-many relationship with User will be configured in UserConfiguration
        }
    }
}
