using Application.DTOs.Route;
using Application.Services.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Routes.Queries
{
    public class GetNearbyRoutesQueryHandler : IRequestHandler<GetNearbyRoutesQuery, List<RouteDto>>
    {
        private readonly IRouteRepository _routeRepository;

        public GetNearbyRoutesQueryHandler(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<List<RouteDto>> Handle(GetNearbyRoutesQuery request, CancellationToken cancellationToken)
        {
            // Ejecutamos la búsqueda espacial (500 metros por defecto)
            var routes = await _routeRepository.GetNearbyRoutesAsync(request.Longitude, request.Latitude);

            // Mapeamos las entidades de Dominio (Route) a DTOs
            return routes.Select(r => new RouteDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }
    }
}