using Application.Services.Interfaces.Repositories;
using Domain.Common;
using Domain.Entities;
using Infraestructur.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infraestructur.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<UserCredential> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<UserCredential> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var credential = await _userManager.FindByEmailAsync(email);
            return credential == null ? null : await GetDomainUserAsync(credential.Id);
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var guidId)) return null;
            var credential = await _userManager.FindByIdAsync(userId);
            return credential == null ? null : await GetDomainUserAsync(credential.Id);
        }

        public async Task<Result> CreateAsync(User user, string password)
        {
            var credential = CreateCredential(user);
            var identityResult = await _userManager.CreateAsync(credential, password);

            if (identityResult.Succeeded)
            {
                return Result.Success();
            }

            var error = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure(error);
        }

        public async Task<Result> CreateAsyncWithThirdParty(User user)
        {
            // 1. Prepare the infrastructure model
            var credential = CreateCredential(user);

            // 2. Execute via UserManager (returns IdentityResult)
            var identityResult = await _userManager.CreateAsync(credential);

            // 3. Map to your Domain.Common.Result
            if (identityResult.Succeeded)
            {
                return Result.Success();
            }

            // Combine Identity errors (e.g., "User already exists") into one string
            var errorMessages = string.Join(", ", identityResult.Errors.Select(e => e.Description));

            return Result.Failure(errorMessages);
        }

        public async Task<Result> ConfirmEmailAsync(User user, string code)
        {
            var credential = await _userManager.FindByIdAsync(user.Id.ToString());
            if (credential == null) return Result.Failure("User credentials not found.");

            var result = await _userManager.ConfirmEmailAsync(credential, code);
            if (result.Succeeded) return Result.Success();

            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(error);
        }

        public async Task<Result> AddToRoleAsync(User user, string roleName)
        {
            var credential = await _userManager.FindByIdAsync(user.Id.ToString());
            if (credential == null) return Result.Failure("User credentials not found.");

            var result = await _userManager.AddToRoleAsync(credential, roleName);
            if (result.Succeeded) return Result.Success();

            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(error);
        }

        public async Task<Result> UpdateAsync(User user)
        {
            // ERROR CORREGIDO: Usamos DomainUsers para la entidad de dominio
            _context.DomainUsers.Update(user);

            var credential = await _userManager.FindByIdAsync(user.Id.ToString());
            if (credential != null)
            {
                credential.Email = user.Email;
                credential.UserName = user.Email;
                await _userManager.UpdateAsync(credential);
            }

            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var credential = await _userManager.FindByIdAsync(user.Id.ToString());
            return credential != null && await _userManager.CheckPasswordAsync(credential, password);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            var credential = await _userManager.FindByIdAsync(user.Id.ToString());
            return credential != null ? await _userManager.GetClaimsAsync(credential) : new List<Claim>();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string token)
        {
            // ERROR CORREGIDO: Usamos el set específico para no confundir tipos
            var credential = await _context.Set<UserCredential>()
                .FirstOrDefaultAsync(c => c.RefreshToken == token);

            return credential == null ? null : await GetDomainUserAsync(credential.Id);
        }

        // --- PRIVATE HELPERS ---

        private async Task<User?> GetDomainUserAsync(Guid id)
    {
        // ERROR CORREGIDO: Usamos DomainUsers. Ahora .Include(u => u.Roles) funcionará
        // porque la clase Domain.User SÍ tiene la propiedad Roles.
        return await _context.DomainUsers
            .Include(u => u.Roles) 
            .FirstOrDefaultAsync(u => u.Id == id);
    }

        private UserCredential CreateCredential(User user)
        {
            return new UserCredential
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                User = user
            };
        }
    }
}