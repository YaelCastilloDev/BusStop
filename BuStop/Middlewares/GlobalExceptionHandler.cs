// --- WebApi/Middlewares/GlobalExceptionHandler.cs ---
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Registramos el error exacto en la consola para depuración
            _logger.LogError(exception, " Ocurrió un error no manejado: {Message}", exception.Message);

            // 2. Construimos la respuesta estándar (RFC 9110)
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                // Nota: En un entorno de Producción real, no deberías mostrar 'exception.Message' 
                // para no revelar información sensible de tu base de datos o código.
                Detail = "Ha ocurrido un error inesperado al procesar tu solicitud.",
                Instance = httpContext.Request.Path
            };

            // 3. Escribimos la respuesta HTTP
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            // 4. Retornamos 'true' para decirle a .NET: "Ya me encargué del error, no rompas la app"
            return true;
        }
    }
}