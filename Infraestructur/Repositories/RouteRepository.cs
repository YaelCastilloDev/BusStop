using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Route?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Routes
                .Include(r => r.Stops) // Load stops with the route
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}