using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Data.Configurations
{
    public class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            builder.ToTable("user_identities");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.Provider).HasColumnName("provider").HasMaxLength(45);
            builder.Property(e => e.ProviderUserId).HasColumnName("provider_user_id").HasMaxLength(255);
            builder.Property(e => e.UsersId).HasColumnName("users_id").HasConversion<byte[]>();

            builder.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("fk_user_identities_users1");
        }
    }
}
