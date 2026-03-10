// --- Application/DTOs/Route/RouteDto.cs ---
namespace Application.DTOs.Route
{
    public class RouteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Puedes añadir más propiedades aquí si tu tabla Route las tiene
    }
}