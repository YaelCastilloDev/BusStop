// --- Application/Features/Stops/Commands/RegisterStopCommand.cs ---
using Application.DTOs.Route;
using MediatR;

namespace Application.Features.Stops.Commands
{
    public record RegisterStopCommand(
        Guid RouteId,
        List<CoordinateDto> RouteCoordinates, // ✨ CAMBIO: Ahora es una lista simple
        Guid CreatedBy
    ) : IRequest<Guid>;
}