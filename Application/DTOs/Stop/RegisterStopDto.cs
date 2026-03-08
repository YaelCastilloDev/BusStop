// --- Application/DTOs/Route/RegisterStopDto.cs ---
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Route
{
    public record CoordinateDto(double Longitude, double Latitude);

    public class RegisterStopDto
    {
        [Required]
        public Guid RouteId { get; set; }

        [Required]
        // ¡Mucho más limpio! Una sola lista continua de puntos.
        public List<CoordinateDto> RouteCoordinates { get; set; } = new();
    }
}