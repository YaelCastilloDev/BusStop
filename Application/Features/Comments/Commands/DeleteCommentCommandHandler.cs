// --- Application/Features/Comments/Commands/DeleteCommentCommandHandler.cs ---
using Application.Services.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Comments.Commands
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly ICommentsRepository _commentsRepository;

        public DeleteCommentCommandHandler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            // 1. Buscamos el comentario en la base de datos
            var comment = await _commentsRepository.GetByIdAsync(request.CommentId);

            // 2. Verificamos si existe y si NO ha sido borrado ya
            if (comment == null || comment.DeletedAt != null)
            {
                return false; // Puedes lanzar una excepción personalizada aquí si lo prefieres
            }

            // 3. (Opcional pero recomendado) Verificar permisos. 
            // ¿El usuario que intenta borrar es el dueño del comentario o es un Moderador/Admin?
            // if (comment.UsersId != request.UserId) throw new UnauthorizedAccessException();

            // 4. Aplicar el Soft Delete
            comment.DeletedAt = DateTime.UtcNow;
            comment.DeletedBy = request.UserId;

            // 5. Guardar los cambios
            await _commentsRepository.UpdateAsync(comment);

            return true;
        }
    }
}