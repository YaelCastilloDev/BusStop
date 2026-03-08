// --- Infraestructur/Repositories/CommentReactionRepository.cs ---
using Application.Services.Interfaces.Comments;
using Domain.Entities;
using Infraestructur.Data; // Asegúrate de que coincida con tu ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructur.Repositories
{
    public class CommentReactionRepository : ICommentReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- Infraestructur/Repositories/CommentReactionRepository.cs ---

        public async Task<bool> UpsertReactionAsync(CommentReaction reaction, CancellationToken cancellationToken = default)
        {
            var existingReaction = await _context.CommentReactions
                .FirstOrDefaultAsync(r => r.CommentId == reaction.CommentId && r.UserId == reaction.UserId, cancellationToken);

            if (existingReaction != null)
            {
                if (existingReaction.Liked == reaction.Liked)
                {
                    _context.CommentReactions.Remove(existingReaction);
                }
                else
                {
                    existingReaction.Liked = reaction.Liked;
                    _context.CommentReactions.Update(existingReaction);
                }
            }
            else
            {
                await _context.CommentReactions.AddAsync(reaction, cancellationToken);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}