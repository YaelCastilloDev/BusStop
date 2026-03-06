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
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.RouteId).HasColumnName("routes_id").HasConversion<byte[]>();
            builder.Property(e => e.RoutePath).HasColumnName("route").HasColumnType("multilinestring");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasConversion<byte[]>();

            builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp") // O datetime, dependiendo de tu preferencia
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

            builder.HasOne(d => d.Route)
                .WithMany(p => p.Stops)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_stops_routes1");
        }
    }
}
