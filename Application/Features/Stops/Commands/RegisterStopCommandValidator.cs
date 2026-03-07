using FluentValidation;

namespace Application.Features.Stops.Commands;

public class RegisterStopCommandValidator : AbstractValidator<RegisterStopCommand>
{
    public RegisterStopCommandValidator()
    {
        // 1. Validate the Route ID
        RuleFor(x => x.RouteId)
            .NotEmpty()
            .WithMessage("The Route ID cannot be empty.");

        // 2. Validate the outer list (MultiLineString must have at least one LineString)
        RuleFor(x => x.RouteCoordinates)
            .NotEmpty()
            .WithMessage("You must provide at least one route segment.");

        // 3. THE CRASH PREVENTER: Validate the inner lists (Each LineString must have >= 2 points)
        RuleForEach(x => x.RouteCoordinates)
            .Must(line => line != null && line.Count >= 2)
            .WithMessage("Each route segment (LineString) must contain at least 2 coordinate points to form a valid line.");

        // 4. Validate the actual geographic values of every single point
        RuleForEach(x => x.RouteCoordinates)
            .ForEach(pointRule =>
            {
                pointRule.Must(p => p.Latitude >= -90 && p.Latitude <= 90)
                    .WithMessage("Latitude must be a valid real-world value between -90 and 90 degrees.");

                pointRule.Must(p => p.Longitude >= -180 && p.Longitude <= 180)
                    .WithMessage("Longitude must be a valid real-world value between -180 and 180 degrees.");
            });
    }
}