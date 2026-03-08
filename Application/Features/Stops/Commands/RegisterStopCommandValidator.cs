// --- Application/Features/Stops/Commands/RegisterStopCommandValidator.cs ---
using FluentValidation;

namespace Application.Features.Stops.Commands;

public class RegisterStopCommandValidator : AbstractValidator<RegisterStopCommand>
{
    public RegisterStopCommandValidator()
    {
        RuleFor(x => x.RouteId)
            .NotEmpty().WithMessage("The Route ID cannot be empty.");

        RuleFor(x => x.RouteCoordinates)
            .NotEmpty().WithMessage("You must provide the route coordinates.")
            // Solo verificamos que la lista tenga al menos 2 puntos para formar una línea
            .Must(list => list != null && list.Count >= 2)
            .WithMessage("The route path must contain at least 2 coordinate points to form a valid line.");

        // Validamos cada punto individualmente de forma mucho más limpia
        RuleForEach(x => x.RouteCoordinates).ChildRules(point =>
        {
            point.RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude must be a valid real-world value between -90 and 90 degrees.");

            point.RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be a valid real-world value between -180 and 180 degrees.");
        });
    }
}