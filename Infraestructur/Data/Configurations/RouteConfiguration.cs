// --- Infrastructur/Data/Configurations/RouteConfiguration.cs ---
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("routes");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100); // Added from SQL
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasConversion<byte[]>();
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasConversion<byte[]>();
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");

            builder.HasOne(d => d.CreatedByUser)
                .WithMany(p => p.CreatedRoutes)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_routes_users1")
                .OnDelete(DeleteBehavior.NoAction);

            // Added this relationship for the 'deleted_by' foreign key
            builder.HasOne(d => d.DeletedByUser)
                .WithMany(p => p.DeletedRoutes)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fk_routes_users_deleted") // Using an assumed FK name
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}