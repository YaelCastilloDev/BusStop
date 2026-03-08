// --- Infraestructur/Repositories/CommentsRepository.cs ---
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur.Data; // Update to match your ApplicationDbContext namespace
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructur.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return comment.Id;
        }
    }
}