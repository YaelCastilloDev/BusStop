// --- Application/Features/Comments/Commands/ReactToCommentCommandHandler.cs ---
using Application.Services.Interfaces.Comments;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands
{
    public class ReactToCommentCommandHandler : IRequestHandler<ReactToCommentCommand, bool>
    {
        private readonly ICommentReactionRepository _repository;

        public ReactToCommentCommandHandler(ICommentReactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ReactToCommentCommand request, CancellationToken cancellationToken)
        {
            var reaction = new CommentReaction
            {
                CommentId = request.CommentId,
                UserId = request.UserId,
                Liked = request.Liked
            };

            return await _repository.UpsertReactionAsync(reaction, cancellationToken);
        }
    }
}