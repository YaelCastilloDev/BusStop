// --- Infrastructure/Data/Configurations/CommentReactionConfiguration.cs ---
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.ToTable("comments_reactions");

            builder.HasKey(e => new { e.UserId, e.CommentId });

            // ELIMINADO: .HasConversion<byte[]>()
            builder.Property(e => e.CommentId).HasColumnName("comments_id");
            builder.Property(e => e.UserId).HasColumnName("users_id");

            builder.Property(e => e.Liked).HasColumnName("liked");

            builder.HasOne(d => d.Comment)
                .WithMany(p => p.Reactions)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("fk_comments_reactions_comments1")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(d => d.User)
                .WithMany(p => p.CommentReactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_comments_reactions_users1")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}