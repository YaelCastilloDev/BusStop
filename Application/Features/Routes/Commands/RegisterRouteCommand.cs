using Application.DTOs.Route;
using MediatR;

namespace Application.Features.Routes.Commands
{
    public record RegisterRouteCommand(
        string Name,
        Guid IdCreator
    ) : IRequest<Guid>;
}