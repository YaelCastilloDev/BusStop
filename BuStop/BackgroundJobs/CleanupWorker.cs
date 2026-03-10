// --- WebApi/BackgroundJobs/CleanupWorker.cs ---
using Application.Services.Interfaces.BackgroundJobs;

namespace WebApi.BackgroundJobs
{
    public class CleanupWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupWorker> _logger;

        // We inject IServiceProvider because BackgroundServices are Singletons (live forever),
        // but our DbContext is Scoped (lives per request). We need to create a scope manually.
        public CleanupWorker(IServiceProvider serviceProvider, ILogger<CleanupWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("⚙️ Cleanup Worker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Calculate time until next 3:00 AM
                    var now = DateTime.Now;
                    var nextRun = DateTime.Today.AddDays(1).AddHours(3); // 3:00 AM tomorrow

                    // If you start the app at 1:00 AM, run it today at 3:00 AM instead of tomorrow
                    if (now.Hour < 3)
                    {
                        nextRun = DateTime.Today.AddHours(3);
                    }

                    var delay = nextRun - now;
                    _logger.LogInformation("⏳ Next cleanup scheduled in {Hours} hours at {NextRun}", delay.TotalHours, nextRun);

                    // 2. Sleep until 3:00 AM
                    // await Task.Delay(delay, stoppingToken);
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);


                    // 3. Wake up and create a scope to get our scoped services
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var cleanupService = scope.ServiceProvider.GetRequiredService<IDatabaseCleanupService>();
                        await cleanupService.ProcessCleanupAsync(stoppingToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    // This is normal when the application shuts down
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ An error occurred while executing the cleanup job. Retrying tomorrow.");
                    // We catch the error so the while-loop doesn't crash permanently.
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}