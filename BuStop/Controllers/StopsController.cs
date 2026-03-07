// --- WebApi/Controllers/StopsController.cs ---
using Application.DTOs.Route;
using Application.Features.Stops.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires JWT
    public class StopsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StopsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> RegisterStop([FromBody] RegisterStopDto dto)
        {
            // 1. Extract User ID from JWT token
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                           ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found or invalid in token", userIdClaim});
            }

            // 2. Create MediatR Command
            var command = new RegisterStopCommand(dto.RouteId, dto.RouteCoordinates, userId);

            // 3. Send Command
            var stopId = await _mediator.Send(command);

            // 4. Return Success
            return CreatedAtAction(null, new { StopId = stopId, Message = "Stop registered successfully" });
        }
    }
}