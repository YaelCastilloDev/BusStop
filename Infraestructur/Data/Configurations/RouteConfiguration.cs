using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("routes");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");

        builder.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");

        builder.HasOne(d => d.Creator)
            .WithMany(p => p.CreatedRoutes)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("fk_routes_users1")
            .OnDelete(DeleteBehavior.NoAction);
    }
}