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
            // 1. Convert DTO Coordinates to NTS MultiLineString
            // We trust the validator, so we can use a clean LINQ chain
            var multiLineString = CreateMultiLineString(request.RouteCoordinates);

            // 2. Map to Domain Entity
            var stop = new Stop
            {
                Id = Guid.NewGuid(),
                RouteId = request.RouteId,
                RoutePath = multiLineString,
                CreatedBy = request.CreatedBy
            };

            // 3. Save to DB
            return await _stopRepository.AddAsync(stop, cancellationToken);
        }

        // Helper Method
        private MultiLineString CreateMultiLineString(List<List<CoordinateDto>> coordinateLists)
        {
            // Because FluentValidation guarantees >= 2 points, 
            // we can safely map directly to LineString objects.
            var lineStrings = coordinateLists
                .Select(line => new LineString(
                    line.Select(c => new Coordinate(c.Longitude, c.Latitude)).ToArray()
                ))
                .ToArray();

            return new MultiLineString(lineStrings);
        }
    }
}