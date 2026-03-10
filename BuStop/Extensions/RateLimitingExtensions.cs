// --- WebApi/Extensions/RateLimitingExtensions.cs ---
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Si alguien se pasa del límite, le devolvemos un 429 Too Many Requests
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // POLÍTICA 1: Global (Para uso general de la API)
                // Permite 60 peticiones cada 1 minuto.
                options.AddFixedWindowLimiter("GlobalPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 20;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });

                // Permite solo 5 intentos cada minuto.
                options.AddFixedWindowLimiter("StrictPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 6;
                    opt.QueueLimit = 0;
                });
            });

            return services;
        }
    }
}