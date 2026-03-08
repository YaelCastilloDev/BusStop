// --- Application/Features/Comments/Commands/CreateCommentCommandHandler.cs ---
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
    {
        private readonly ICommentsRepository _repository;

        public CreateCommentCommandHandler(ICommentsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                RouteId = request.RouteId,
                UserId = request.UserId,
                Text = request.Text
                // CreatedAt or DeletedAt are handled by DB defaults and configurations
            };

            return await _repository.AddAsync(comment, cancellationToken);
        }
    }
}