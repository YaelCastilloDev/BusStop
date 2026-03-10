using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infraestructur.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RouteRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Route> AddAsync(Route route)
        {
            await _dbContext.Routes.AddAsync(route);
            await _dbContext.SaveChangesAsync();
            return route;
        }

        public async Task<Route?> GetByIdWithStopsAsync(Guid id)
        {
            return await _dbContext.Routes
                .Include(r => r.Stops) // Load stops with the route
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Route>> GetNearbyRoutesAsync(double longitude, double latitude, double radiusInMeters = 500)
        {
            // 1. Usar SRID 0 (Plano Cartesiano) en lugar de 4326
            var userLocation = new Point(longitude, latitude) { SRID = 0 };

            // 2. Convertir los metros a grados. 
            // 500 metros / 111100 = ~0.0045 grados.
            double radiusInDegrees = radiusInMeters / 111100.0;

            // 3. Buscar usando grados
            var nearbyRoutes = await _dbContext.Routes
                .Where(r => r.Stops.Any(s => s.RoutePath.Distance(userLocation) <= radiusInDegrees))
                .ToListAsync();

            if (nearbyRoutes.Any())
            {
                return nearbyRoutes;
            }

            // 4. Fallback: Buscar la más cercana (el ordenamiento por distancia Euclidiana funciona igual)
            var nearestRoute = await _dbContext.Routes
                .Where(r => r.Stops.Any())
                .OrderBy(r => r.Stops.Min(s => s.RoutePath.Distance(userLocation)))
                .FirstOrDefaultAsync();

            return nearestRoute != null ? new List<Route> { nearestRoute } : new List<Route>();
        }

        public async Task<Route?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Routes.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Route route)
        {
            _dbContext.Routes.Update(route);
            await _dbContext.SaveChangesAsync();
        }
    }
}