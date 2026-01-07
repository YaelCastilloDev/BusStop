using Application.DTOs.Route;
using MediatR;

namespace Application.Features.Routes.Commands
{
    public record RegisterRouteCommand(
        string Name,
        string? Description,
        List<RegisterStopDto> Stops,
        Guid UserId 
    ) : IRequest<Guid>;
}