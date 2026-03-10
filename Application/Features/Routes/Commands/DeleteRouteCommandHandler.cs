// --- Application/Features/Routes/Commands/DeleteRouteCommandHandler.cs ---
using Application.Services.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Routes.Commands
{
    public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, bool>
    {
        private readonly IRouteRepository _routeRepository;

        public DeleteRouteCommandHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
        {
            var route = await _routeRepository.GetByIdAsync(request.RouteId);

            // Verificamos si la ruta existe y si no está borrada ya
            if (route == null || route.DeletedAt != null)
            {
                return false;
            }

            // Aplicamos el Soft Delete
            route.DeletedAt = DateTime.UtcNow;
            route.DeletedBy = request.UserId;

            await _routeRepository.UpdateAsync(route);

            return true;
        }
    }
}