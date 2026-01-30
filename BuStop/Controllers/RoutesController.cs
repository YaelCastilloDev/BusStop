using Application.DTOs.Route;
using Application.Features.Routes.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoutesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterRoute([FromBody] RegisterRouteDto dto)
        {
            // 1. Get User ID safely from Token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value; // 'sub' is standard for OIDC/Google

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }

            // 2. Create the Command (Mapping DTO + Token Data)
            var command = new RegisterRouteCommand(
                dto.Name,
                dto.Description,
                dto.Stops,
                userId
            );

            // 3. Send to Handler
            var routeId = await _mediator.Send(command);

            // 4. Return Created (201) status for new resources
            return CreatedAtAction(nameof(RegisterRoute), new { id = routeId }, new
            {
                RouteId = routeId,
                Message = "Route registered successfully"
            });
        }
    }
}