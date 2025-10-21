using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(e => e.Id);

            // BINARY(16) to Guid conversion
            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.RoleId).HasColumnName("roles_id");

            // FK: fk_users_roles (User -> Role)
            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_users_roles")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
