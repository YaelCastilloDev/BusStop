using Application.DTOs.Auth;

namespace Application.Services.Interfaces.Authentication
{
    public interface IAuthResponse

    {
        Task<AuthResponse> SignUpAsync(SignUp model, string orgin);

        Task<AuthResponse> LoginAsync(Login model);

        Task<string> AssignRolesAsync(AssignRoles model);

        Task<AuthResponse> RefreshTokenCheckAsync(string token);

        Task<bool> RevokeTokenAsync(string token);

        Task<string> ConfirmEmailAsync(string userId, string code);

    }
}
