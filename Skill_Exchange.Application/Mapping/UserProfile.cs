using AutoMapper;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
            CreateMap<CreateUserDTO, AppUser>().ReverseMap();
            CreateMap<AppUser, CreateUserResponseDTO>()
             .ForMember(dest => dest.userDTO, opt => opt.MapFrom(src => src)).ReverseMap();

            //CreateMap<UpdateDTO, AppUser>().ReverseMap();
        }
    }
}
