using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Mapping
{
    public class UserSkill : Profile
    {
        public UserSkill()
        {
            CreateMap<AddUserSkillDTO, UserSkills>().ReverseMap();

            CreateMap<UserSkills, UserSkillsDTO>().ReverseMap();

            CreateMap<UserSkills, UserSkillDTO>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name))
                .ForMember(dest => dest.SkillCategoryName, opt => opt.MapFrom(src => src.Skill.SkillCategory.Name))
                .ReverseMap();

            CreateMap<UserSkills, UserWithSkillInfoDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.User.Bio))
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
            .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.YearsOfExperience))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Purpose, opt => opt.MapFrom(src => src.Purpose));


        }
    }
}