// --- Application/Features/Comments/Commands/CreateCommentCommand.cs ---
using MediatR;
using System;

namespace Application.Features.Comments.Commands
{
    // Returns the new Comment's ID
    public record CreateCommentCommand(Guid RouteId, string Text, Guid UserId) : IRequest<Guid>;
}