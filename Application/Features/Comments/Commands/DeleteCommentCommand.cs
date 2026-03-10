// --- Application/Features/Comments/Commands/DeleteCommentCommand.cs ---
using MediatR;

namespace Application.Features.Comments.Commands
{
    // Recibimos el ID del comentario y el ID del usuario que está solicitando borrarlo
    public record DeleteCommentCommand(Guid CommentId, Guid UserId) : IRequest<bool>;
}