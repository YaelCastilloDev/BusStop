// --- Application/Services/Interfaces/BackgroundJobs/IDatabaseCleanupService.cs ---
namespace Application.Services.Interfaces.BackgroundJobs
{
    public interface IDatabaseCleanupService
    {
        Task ProcessCleanupAsync(CancellationToken cancellationToken);
    }
}