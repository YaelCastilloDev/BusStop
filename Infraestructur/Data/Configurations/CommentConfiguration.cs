using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");
            builder.HasKey(e => e.Id);

            // BINARY(16) to Guid conversion and column names
            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasConversion<byte[]>();
            builder.Property(e => e.UserId).HasColumnName("users_id").HasConversion<byte[]>();
            builder.Property(e => e.RouteId).HasColumnName("routes_id").HasConversion<byte[]>();
            builder.Property(e => e.DeletedAt).HasColumnType("timestamp");

            // FK: fk_comments_users1 (Comment -> User)
            builder.HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_comments_users1")
                .OnDelete(DeleteBehavior.NoAction);

            // FK: fk_comments_routes1 (Comment -> Route)
            builder.HasOne(d => d.Route)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_comments_routes1")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
