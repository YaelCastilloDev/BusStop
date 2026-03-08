// --- Application/Features/Comments/Commands/CreateCommentCommandValidator.cs ---
using FluentValidation;

namespace Application.Features.Comments.Commands
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty().WithMessage("The Route ID is required.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("The comment text cannot be empty.")
                .MaximumLength(500).WithMessage("The comment cannot exceed 500 characters.");
        }
    }
}