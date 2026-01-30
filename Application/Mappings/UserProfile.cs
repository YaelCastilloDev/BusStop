using Application.DTOs.Auth;
using AutoMapper;
using Domain.Entities; // Your corrected User entity

namespace Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpDto, User>()
                .ForMember(
                    dest => dest.Name, // Change from UserName to Name
                    opt => opt.MapFrom(src => src.Name)
                )
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email)
                );
        }
    }
}