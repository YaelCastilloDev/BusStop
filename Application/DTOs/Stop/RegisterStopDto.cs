// --- Application/DTOs/Route/RegisterStopDto.cs ---
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Route
{
    // A simple record to hold Longitude (X) and Latitude (Y)
    public record CoordinateDto(double Longitude, double Latitude);

    public class RegisterStopDto
    {
        [Required]
        public Guid RouteId { get; set; }

        [Required]
        // Outer List = The MultiLineString
        // Inner List = The LineStrings (Paths)
        // CoordinateDto = The Points
        public List<List<CoordinateDto>> RouteCoordinates { get; set; } = new();
    }
}