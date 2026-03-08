// --- Application/Features/Stops/Commands/RegisterStopCommandHandler.cs ---
using Application.DTOs.Route;
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using NetTopologySuite.Geometries;

namespace Application.Features.Stops.Commands
{
    public class RegisterStopCommandHandler : IRequestHandler<RegisterStopCommand, Guid>
    {
        private readonly IStopRepository _stopRepository;

        public RegisterStopCommandHandler(IStopRepository stopRepository)
        {
            _stopRepository = stopRepository;
        }

        public async Task<Guid> Handle(RegisterStopCommand request, CancellationToken cancellationToken)
        {
            // 1. Convert DTO Coordinates to NTS LineString
            var lineString = CreateLineString(request.RouteCoordinates);

            // 2. Map to Domain Entity
            var stop = new Stop
            {
                Id = Guid.NewGuid(),
                RouteId = request.RouteId,
                RoutePath = lineString, // ✨ Asignamos la línea continua
                CreatedBy = request.CreatedBy
            };

            // 3. Save to DB
            return await _stopRepository.AddAsync(stop, cancellationToken);
        }

        // ✨ NUEVO Helper Method
        private LineString CreateLineString(List<CoordinateDto> coordinates)
        {
            var ntsCoordinates = coordinates
                .Select(c => new Coordinate(c.Longitude, c.Latitude))
                .ToArray();

            return new LineString(ntsCoordinates);
        }
    }
}