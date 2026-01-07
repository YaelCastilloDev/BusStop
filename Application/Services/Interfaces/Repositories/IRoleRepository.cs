using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        public Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
