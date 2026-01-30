using Domain.Entities;
using Domain.Common; // Add this
using System.Security.Claims;

namespace Application.Services.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email);

        // Return Result instead of bool or IdentityResult
        Task<Result> CreateAsync(User user, string password);
         Task<Result> CreateAsyncWithThirdParty(User user);

        Task<Result> ConfirmEmailAsync(User user, string code);


        Task<Result> AddToRoleAsync(User user, string roleName);
        Task<Result> UpdateAsync(User user);

        // Queries stay mostly the same
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<Claim>> GetClaimsAsync(User user);
        Task<User?> GetUserByRefreshTokenAsync(string token);
        Task<User?> FindByIdAsync(string userId);
    }
}