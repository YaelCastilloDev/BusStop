using AutoMapper;
using Domain; // Allowed: Infrastructure can reference Domain
using Infraestructur.Identity.Models; // Allowed: Infrastructure referencing its own models

namespace Infraestructur.Models
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            // Mapping from Infrastructure Model (AppUser) to Domain Entity (User)
            CreateMap<AppUser, User>()
                .ReverseMap();
            // AutoMapper handles mapping the shared properties (Id, Name, Email, RoleId).
            // It automatically ignores the extra 'refreshTokens' property when mapping 
            // from AppUser to User, as 'User' doesn't have it.
        }
    }
}
