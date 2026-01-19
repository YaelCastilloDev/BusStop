using AutoMapper;
using Domain; 
using Domain.Entities;
using Infraestructur.Identity.Models; 

namespace Infraestructur.Models
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            // Mapping from Infrastructure Model (AppUser) to Domain Entity (User)
            CreateMap<UserIdentity, User>()
                .ReverseMap();

        }
    }
}
