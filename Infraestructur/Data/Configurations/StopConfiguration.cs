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
            builder.
            // Composite Primary Key (id, stop_types_bus_stop)
            HasKey(e => new { e.Id, e.StopTypeBusStopId });

            // BINARY(16) to Guid conversion and column names
            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.PreviousStopId).HasColumnName("previous_stop").HasConversion<byte[]>();
            builder.Property(e => e.NextStopId).HasColumnName("next_stop").HasConversion<byte[]>();
            builder.Property(e => e.RouteId).HasColumnName("routes_id").HasConversion<byte[]>();
            builder.Property(e => e.StopTypeBusStopId).HasColumnName("stop_types_bus_stop");

            // Map POINT type (Requires NetTopologySuite)
            builder.Property(e => e.Location).HasColumnName("location").HasColumnType("point").IsRequired();

            // FK: fk_stops_routes1 (Stop -> Route)
            builder.HasOne(d => d.Route)
                .WithMany(p => p.Stops)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_stops_routes1")
                .OnDelete(DeleteBehavior.NoAction);

            // FK: fk_stops_stop_types1 (Stop -> StopType)
            builder.HasOne(d => d.StopType)
                .WithMany(p => p.Stops)
                .HasForeignKey(d => d.StopTypeBusStopId)
                .HasConstraintName("fk_stops_stop_types1")
                .OnDelete(DeleteBehavior.NoAction);

            // NOTE: Self-referencing keys (PreviousStop/NextStop) are often complex 
            // and might be better handled in separate relationships if required for navigation.
            // Since they are only columns here, they are mapped as simple properties.
        }
    }
}
