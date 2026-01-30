using Application.Services.Interfaces.Repositories;
using Infraestructur.Data;
using Infraestructur.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infraestructur.Repositories
{
    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly ApplicationDbContext _context;

        public UserIdentityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddIdentityAsync(Guid userId, string provider, string providerUserId)
        {
            var identity = new UserIdentity
            {
                Id = Guid.NewGuid(),
                UsersId = userId,
                Provider = provider,
                ProviderUserId = providerUserId
                // EF Core will handle the relationship via UsersId
            };

            await _context.UserIdentities.AddAsync(identity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByProviderAsync(Guid userId, string provider)
        {
            return await _context.UserIdentities
                .AnyAsync(x => x.UsersId == userId && x.Provider == provider);
        }
    }
}