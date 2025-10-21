using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructur.Data.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.ToTable("comments_reactions");

            // Primary Key is users_id
            builder.HasKey(e => e.UserId);

            // BINARY(16) to Guid conversion and column names
            builder.Property(e => e.UserId).HasColumnName("users_id").HasConversion<byte[]>();
            builder.Property(e => e.CommentId).HasColumnName("comments_id").HasConversion<byte[]>();
            builder.Property(e => e.Liked).HasColumnType("tinyint");

            // FK: fk_comments_reactions_comments1 (Reaction -> Comment)
            builder.HasOne(d => d.Comment)
                .WithMany(p => p.Reactions)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("fk_comments_reactions_comments1")
                .OnDelete(DeleteBehavior.NoAction);

            // FK: fk_comments_reactions_users1 (Reaction -> User) - One-to-one relationship
            builder.HasOne(d => d.User)
                .WithOne(p => p.CommentReaction)
                .HasForeignKey<CommentReaction>(d => d.UserId)
                .HasConstraintName("fk_comments_reactions_users1")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
