using Domain.Entities;
using Microsoft.AspNetCore.Identity; // It's OK for Application to reference Identity's core types
using System.Security.Claims;

namespace Application.Services.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Methods needed for SignUp
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByNameAsync(string name);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        // Methods needed for Login
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<Claim>> GetClaimsAsync(User user);

        // Methods needed for Refresh Token
        Task<User?> GetUserByRefreshTokenAsync(string token);

        // Methods needed for Role Assignment
        Task<User?> FindByIdAsync(string userId);
        Task<bool> IsInRoleAsync(User user, string roleName);

        // General Methods
        Task UpdateAsync(User user);
        Task<IList<string>> GetRolesAsync(User user);
        Task AddToRoleAsync(User user, string roleName);
        Task<IdentityResult> ConfirmEmailAsync(User user, string code);
    }
}