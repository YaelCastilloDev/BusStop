// --- Application/Features/Routes/Commands/DeleteRouteCommand.cs ---
using MediatR;

namespace Application.Features.Routes.Commands
{
    public record DeleteRouteCommand(Guid RouteId, Guid UserId) : IRequest<bool>;
}