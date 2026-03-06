using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using NetTopologySuite.Geometries; 
namespace Application.Features.Routes.Commands
{
    public class RegisterRouteCommandHandler : IRequestHandler<RegisterRouteCommand, Guid>
    {
        private readonly IRouteRepository _routeRepository;

        public RegisterRouteCommandHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<Guid> Handle(RegisterRouteCommand request, CancellationToken cancellationToken)
        {
            var route = new Route
            {
                Id = Guid.NewGuid(), // Server-generated ID
                Name = request.Name,
                CreatedBy = request.IdCreator,
                // Description = request.Description // Add this to the Command/Entity if needed
            };

            await _routeRepository.AddAsync(route);

            return route.Id;
        }
    }
}