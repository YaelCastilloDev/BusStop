using Application.DTOs.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login.GoogleLogin
{
    // 👇 ¡ESTA PARTE ES LA QUE FALTA O ESTÁ INCORRECTA!
    // Debes heredar de IRequest<AuthResponse>
    public record GoogleLoginCommand(string IdToken) : IRequest<AuthResponseDto>;
}