// --- Application/Services/Interfaces/Repositories/IStopRepository.cs ---
using Domain.Entities;

namespace Application.Services.Interfaces.Repositories
{
    public interface IStopRepository
    {
        Task<Guid> AddAsync(Stop stop, CancellationToken cancellationToken = default);
    }
}