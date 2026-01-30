using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Route
{
    public class RegisterRouteDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Added this to match your Controller's logic
        // This could be a list of coordinates or Stop IDs
        [Required]
        public List<StopDto> Stops { get; set; } = new();
    }
}
