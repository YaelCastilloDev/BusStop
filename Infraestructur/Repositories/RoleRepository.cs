using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructur.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            // Explicitly referencing Domain.Entities.Role
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task AssignRoleToUserAsync(Guid userId, int roleId)
        {
            // We use .Include(u => u.Roles) to load the ICollection<Role> 
            // defined in your User Domain Entity
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var role = await _context.Set<Role>().FindAsync(roleId);

            if (user != null && role != null)
            {
                if (!user.Roles.Any(r => r.Id == roleId))
                {
                    user.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            // This navigates the many-to-many relationship defined in your classes
            return await _context.Set<Role>()
                .Where(r => r.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }
    }
}