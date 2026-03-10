using Application.DTOs.Route;
using Application.Features.Routes.Commands;
using Application.Features.Routes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory; // ✨ Necesario para el Caché
using System.Security.Claims;
using System.Security.Cryptography; // ✨ Necesario para generar el ETag
using System.Text;
using System.Text.Json;
using WebApi.Common.Caching;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache; // ✨ Inyectamos el caché

        public RoutesController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

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

        [HttpGet("nearby")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNearbyRoutes([FromQuery] double longitude, [FromQuery] double latitude)
        {

            double roundedLon = Math.Round(longitude, 3);
            double roundedLat = Math.Round(latitude, 3);
            string cacheKey = $"NearbyRoutes_{roundedLon}_{roundedLat}";

            if (!_cache.TryGetValue(cacheKey, out CachedData<List<RouteDto>>? cachedData))
            {
                var query = new GetNearbyRoutesQuery(roundedLon, roundedLat);
                var routes = await _mediator.Send(query);

                string eTag = ETagGenerator.Generate(routes);

                // Empaquetamos y guardamos
                cachedData = new CachedData<List<RouteDto>> { Data = routes, ETag = eTag };
                _cache.Set(cacheKey, cachedData, TimeSpan.FromMinutes(5));
            }

            if (Request.Headers.TryGetValue("If-None-Match", out var clientETag) && clientETag == cachedData!.ETag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers.ETag = cachedData!.ETag;

            return Ok(cachedData.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }

            var command = new DeleteRouteCommand(id, userId);
            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound(new { Message = "Route not found or already deleted." });
            }

            return Ok(new { Message = "Route deleted successfully." });
        }
    }
}