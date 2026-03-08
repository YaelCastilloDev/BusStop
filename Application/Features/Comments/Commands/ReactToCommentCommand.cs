// --- Application/Features/Comments/Commands/ReactToCommentCommand.cs ---
using MediatR;
using System;

namespace Application.Features.Comments.Commands
{
    // Devuelve un booleano indicando si la operación fue exitosa
    public record ReactToCommentCommand(Guid CommentId, Guid UserId, bool Liked) : IRequest<bool>;
}