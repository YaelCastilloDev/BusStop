// --- Application/Features/Stops/Commands/RegisterStopCommand.cs ---
using Application.DTOs.Route;
using MediatR;

namespace Application.Features.Stops.Commands
{
    public record RegisterStopCommand(
        Guid RouteId,
        List<List<CoordinateDto>> RouteCoordinates,
        Guid CreatedBy
    ) : IRequest<Guid>; // Returns the ID of the newly created Stop
}