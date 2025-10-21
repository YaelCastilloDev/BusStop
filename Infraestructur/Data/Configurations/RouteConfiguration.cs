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

            // BINARY(16) to Guid conversion and column name mapping
            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasConversion<byte[]>();
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasConversion<byte[]>();
            builder.Property(e => e.DeletedAt).HasColumnType("timestamp");

            // FK: fk_routes_users1 (Route -> User (CreatedBy))
            builder.HasOne(d => d.CreatedByUser)
                .WithMany(p => p.CreatedRoutes)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_routes_users1")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
