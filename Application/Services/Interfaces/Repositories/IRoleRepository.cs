using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        // Find a role entity by its name (e.g., "User", "Admin")
        Task<Role?> GetRoleByNameAsync(string roleName);

        // Create the Many-to-Many link between User and Role
        Task AssignRoleToUserAsync(Guid userId, int roleId);

        // Get all roles assigned to a specific user (needed for claims/token)
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
    }
}