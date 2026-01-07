// --- Infrastructur/Data/Configurations/StopConfiguration.cs ---
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class StopConfiguration : IEntityTypeConfiguration<Stop>
    {
        public void Configure(EntityTypeBuilder<Stop> builder)
        {
            builder.ToTable("stops");

            // Primary key is now just 'Id'
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.RouteId).HasColumnName("routes_id").HasConversion<byte[]>();

            // Map the 'Route' property to the 'route' (multilinestring) column
            builder.Property(e => e.Route)
                .HasColumnName("route")
                .HasColumnType("multilinestring")
                .IsRequired();

            // Add mappings for new audit columns
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasConversion<byte[]>();
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasConversion<byte[]>();

            // --- Removed Property Mappings ---
            // builder.Property(e => e.PreviousStopId)...
            // builder.Property(e => e.NextStopId)...
            // builder.Property(e => e.StopTypeBusStopId)...
            // builder.Property(e => e.Location)...

            // FK to Routes
            builder.HasOne(d => d.RouteNav) // Use the renamed navigation property
                .WithMany(p => p.Stops)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_stops_routes1")
                .OnDelete(DeleteBehavior.NoAction);

            // Added FK to Users for CreatedBy
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(p => p.CreatedStops)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_stops_users1")
                .OnDelete(DeleteBehavior.NoAction);

            // Added FK to Users for DeletedBy
            builder.HasOne(d => d.DeletedByUser)
                .WithMany(p => p.DeletedStops)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fk_stops_users2")
                .OnDelete(DeleteBehavior.NoAction);

            // Removed FK for StopType
            // builder.HasOne(d => d.StopType)...
        }
    }
}
