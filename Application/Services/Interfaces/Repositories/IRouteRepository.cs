using Domain.Entities;

namespace Application.Services.Interfaces.Repositories
{
    public interface IRouteRepository
    {
        // Change RegisterRouteDto to Route
        Task<Route> AddAsync(Route route);
        Task<Route?> GetByIdAsync(Guid id);
    }
}