// --- Application/Services/Interfaces/Comments/ICommentReactionRepository.cs ---
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Comments
{
    public interface ICommentReactionRepository
    {
        Task<bool> UpsertReactionAsync(CommentReaction reaction, CancellationToken cancellationToken = default);
    }
}