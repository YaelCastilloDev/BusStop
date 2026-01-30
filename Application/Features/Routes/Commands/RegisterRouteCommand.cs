using Application.DTOs.Route;
using MediatR;

namespace Application.Features.Routes.Commands
{
    // Using a Record is cleaner for Commands
    public record RegisterRouteCommand(
        string Name,
        string? Description,
        List<StopDto> Stops,
        Guid IdCreator
    ) : IRequest<Guid>;
}