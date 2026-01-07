// --- Infrastructur/Data/Configurations/RoleConfiguration.cs ---
using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(40).IsRequired();

            builder.HasIndex(e => e.Name).IsUnique(); 
        }
    }
}