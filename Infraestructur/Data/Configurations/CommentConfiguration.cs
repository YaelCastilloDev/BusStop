// --- Infrastructur/Data/Configurations/CommentConfiguration.cs ---
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

            builder.Property(e => e.Id).HasColumnName("id").HasConversion<byte[]>();
            builder.Property(e => e.Text).HasColumnName("text").HasMaxLength(500);
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasConversion<byte[]>();
            builder.Property(e => e.UserId).HasColumnName("users_id").HasConversion<byte[]>();
            builder.Property(e => e.RouteId).HasColumnName("routes_id").HasConversion<byte[]>();

            builder.HasOne(d => d.User)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_comments_users1")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.Route)
                .WithMany(p => p.Comments)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("fk_comments_routes1")
                .OnDelete(DeleteBehavior.NoAction);

            // Assumed FK for DeletedBy
            builder.HasOne(d => d.DeletedByUser)
                .WithMany() // A user can delete many comments, but 'User' entity doesn't track it
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fk_comments_users_deleted")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}