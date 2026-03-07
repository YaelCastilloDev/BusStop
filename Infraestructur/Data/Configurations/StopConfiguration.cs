// --- Infrastructur/Data/Configurations/StopConfiguration.cs ---
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class StopConfiguration : IEntityTypeConfiguration<Stop>
    {
        // --- Infrastructur/Data/Configurations/StopConfiguration.cs ---

        // --- Infrastructur/Data/Configurations/StopConfiguration.cs ---

        public void Configure(EntityTypeBuilder<Stop> builder)
        {
            builder.ToTable("stops");
            builder.HasKey(e => e.Id);

            // 1. Añadimos explícitamente HasColumnType("binary(16)") a los GUIDs
            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("binary(16)");

            builder.Property(e => e.RouteId)
                .HasColumnName("routes_id");

            builder.Property(e => e.RoutePath)
                .HasColumnName("route")
                .HasColumnType("multilinestring");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("binary(16)");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp")
                .IsRequired(false);

            // 2. Corregimos DeletedBy (quitamos HasConversion y ponemos HasColumnType)
            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by")
                .IsRequired(false);

            builder.HasOne(d => d.Route)
                .WithMany(p => p.Stops)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_stops_routes1");
        }
    }
}
