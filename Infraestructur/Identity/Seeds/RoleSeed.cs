// --- Infraestructur/Identity/Seeds/RoleSeed.cs ---
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructur.Identity.Seeds
{
    public class RoleSeed : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasData(
                new AppRole
                {
                    Id = Guid.Parse("0195758d-7b2a-7c9e-9f4a-1a2b3c4d5e6f"),
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new AppRole
                {
                    Id = Guid.Parse("0195758d-7b2a-7c9e-9f4b-2b3c4d5e6f7a"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new AppRole
                {
                    Id = Guid.Parse("0195758d-7b2a-7c9e-9f4c-3c4d5e6f7a8b"),
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                },
                new AppRole
                {
                    Id = Guid.Parse("0195758d-7b2a-7c9e-9f4d-4d5e6f7a8b9c"),
                    Name = "BasicUser",
                    NormalizedName = "BASICUSER"
                }
            );
        }
    }
}