// --- Infraestructur/Services/DatabaseCleanupService.cs ---
using Application.Services.Interfaces.BackgroundJobs;
using Infraestructur.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infraestructur.Services
{
    public class DatabaseCleanupService : IDatabaseCleanupService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseCleanupService> _logger;

        public DatabaseCleanupService(ApplicationDbContext context, ILogger<DatabaseCleanupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ProcessCleanupAsync(CancellationToken cancellationToken)
        {
            // Calculate the exact cutoff date (14 days ago)
            var cutoffDate = DateTime.UtcNow.AddMinutes(-1); // for testing //////////////////////////////

            //var cutoffDate = DateTime.UtcNow.AddDays(-14);  // for production //////////////////////////////

            _logger.LogInformation("🧹 Starting database cleanup for records deleted before {CutoffDate}", cutoffDate);

            // IMPORTANT: We MUST use IgnoreQueryFilters() because earlier we told EF Core 
            // to hide records where DeletedAt != null. We need to see them to delete them!

            // 1. Delete Comments
            var deletedCommentsCount = await _context.Comments
                .IgnoreQueryFilters()
                .Where(c => c.DeletedAt != null && c.DeletedAt <= cutoffDate)
                .ExecuteDeleteAsync(cancellationToken);

            // 2. Delete Stops (We delete these before Routes to avoid Foreign Key constraint errors)
            var deletedStopsCount = await _context.Stops
                .IgnoreQueryFilters()
                .Where(s => s.DeletedAt != null && s.DeletedAt <= cutoffDate)
                .ExecuteDeleteAsync(cancellationToken);

            // 3. Delete Routes
            var deletedRoutesCount = await _context.Routes
                .IgnoreQueryFilters()
                .Where(r => r.DeletedAt != null && r.DeletedAt <= cutoffDate)
                .ExecuteDeleteAsync(cancellationToken);

            _logger.LogInformation("✅ Cleanup complete. Deleted: {Comments} Comments, {Stops} Stops, {Routes} Routes.",
                deletedCommentsCount, deletedStopsCount, deletedRoutesCount);
        }
    }
}