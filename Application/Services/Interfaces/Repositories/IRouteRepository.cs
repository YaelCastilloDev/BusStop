using Domain.Entities;

namespace Application.Services.Interfaces.Repositories
{
    public interface IRouteRepository
    {
        // Change RegisterRouteDto to Route
        Task<Route> AddAsync(Route route);
        Task<Route?> GetByIdWithStopsAsync(Guid id);

        Task<List<Route>> GetNearbyRoutesAsync(double longitude, double latitude, double radiusInMeters = 500);
        Task UpdateAsync(Route route);
        Task<Route?> GetByIdAsync(Guid id);
    }
}