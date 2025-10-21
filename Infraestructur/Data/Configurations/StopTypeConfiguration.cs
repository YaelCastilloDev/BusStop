using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class StopTypeConfiguration : IEntityTypeConfiguration<StopType>
    {
        public void Configure(EntityTypeBuilder<StopType> builder)
        {
            builder.ToTable("stop_types");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.StreetRoute).HasColumnName("street_route");
        }
    }
}
