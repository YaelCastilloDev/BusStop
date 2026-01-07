using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infraestructur.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager; // appuser
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context; 
        }

        public async Task<User?> FindByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<User?> FindByNameAsync(string name) =>
            await _userManager.FindByNameAsync(name);

        public async Task<IdentityResult> CreateAsync(User user, string password) =>
            await _userManager.CreateAsync(user, password);
        
        public async Task AddToRoleAsync(User user, string roleName) =>
            await _userManager.AddToRoleAsync(user, roleName);

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) =>
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string code) =>
            await _userManager.ConfirmEmailAsync(user, code);
        
        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);
        
        public async Task<IList<Claim>> GetClaimsAsync(User user) =>
            await _userManager.GetClaimsAsync(user);

        public async Task<IList<string>> GetRolesAsync(User user) =>
            await _userManager.GetRolesAsync(user);

        public async Task UpdateAsync(User user) =>
            await _userManager.UpdateAsync(user);

        public async Task<User?> FindByIdAsync(string userId) =>
            await _userManager.FindByIdAsync(userId);
        
        public async Task<bool> IsInRoleAsync(User user, string roleName) =>
            await _userManager.IsInRoleAsync(user, roleName);

        public async Task<User?> GetUserByRefreshTokenAsync(string token)
        {
            var userIds = await _context.Database
                .SqlQueryRaw<Guid>("SELECT UserId FROM refresh_tokens WHERE Token = {0}", token)
                .ToListAsync();

            var userId = userIds.FirstOrDefault(); 

            if (userId == Guid.Empty)
            {
                return null;
            }

            return await _userManager.FindByIdAsync(userId.ToString());
        }
    }
}