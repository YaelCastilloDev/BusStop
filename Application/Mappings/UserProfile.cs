using Application.DTOs.Auth;
using AutoMapper;
using Domain.Entities; // Your corrected User entity

namespace Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignUp, User>()
                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Name)
                )
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email)
                );


        }
    }
}