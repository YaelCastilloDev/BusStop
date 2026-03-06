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
    [Authorize] // 1. Now that your JWT works, protect this!
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoutesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> RegisterRoute([FromBody] RegisterRouteDto dto)
        {
            // 2. Extract User ID from the JWT Claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }

            // 3. Create Command (Matching the record definition)
            var command = new RegisterRouteCommand(dto.Name, userId);

            // 4. Send to MediatR
            var routeId = await _mediator.Send(command);

            // 5. Return 201 Created
            return Ok(new { RouteId = routeId, Message = "Route registered successfully" });
        }
    }
}