using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.Repositories
{
    public interface IUserIdentityRepository
    {
        // Pass the raw values needed to create the link
        Task AddIdentityAsync(Guid userId, string provider, string providerUserId);

        // Just return true/false. The handler doesn't need the DB object, just needs to know if it exists.
        Task<bool> ExistsByProviderAsync(Guid userId, string provider);
    }
}