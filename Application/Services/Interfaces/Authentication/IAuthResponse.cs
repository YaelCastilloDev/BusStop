using Application.DTOs.Auth;

namespace Application.Services.Interfaces.Authentication
{
    public interface IAuthResponse

    {
        Task<AuthResponseDto> SignUpAsync(SignUpDto model, string orgin);

        Task<AuthResponseDto> LoginAsync(LoginDto model);

        Task<string> AssignRolesAsync(AssignRolesDto model);

        Task<AuthResponseDto> RefreshTokenCheckAsync(string token);

        Task<bool> RevokeTokenAsync(string token);

        Task<string> ConfirmEmailAsync(string userId, string code);

    }
}
