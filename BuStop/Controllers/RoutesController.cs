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
    [Authorize] // Requires JWT Token
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
            // 1. Get User ID from the JWT Token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("userId")?.Value;

            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            // 2. Create the Command
            var command = new RegisterRouteCommand(
                dto.Name,
                dto.Description,
                dto.Stops,
                userId
            );

            // 3. Send to Handler
            var routeId = await _mediator.Send(command);

            // 4. Return Result
            return Ok(new { RouteId = routeId, Message = "Route registered successfully" });
        }
    }
}