using XelerationTask.Application.DTOs;
using XelerationTask.Core.Models;
using AutoMapper;

namespace XelerationTask.Application.Mappers
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile() {

            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsActiveUser, opt => opt.MapFrom(src => !src.IsDeleted));

            CreateMap<(string AccessToken, string RefreshToken), UserLoginResultDTO>()
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken));
        }
    }
}
