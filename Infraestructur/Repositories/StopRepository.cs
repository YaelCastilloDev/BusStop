// --- Infraestructur/Repositories/StopRepository.cs ---
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur.Data; // Ensure this points to your ApplicationDbContext

namespace Infraestructur.Repositories
{
    public class StopRepository : IStopRepository
    {
        private readonly ApplicationDbContext _context;

        public StopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Stop stop, CancellationToken cancellationToken = default)
        {
            await _context.Stops.AddAsync(stop, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return stop.Id;
        }
    }
}