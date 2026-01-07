using Application.DTOs.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public record RegisterUserCommand(
        string Username,
        string Email,
        string Password
    ) : IRequest<AuthResponse>;
}