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

            CreateMap<UserSkills, UserSkillDTO>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name))
                .ForMember(dest => dest.SkillCategoryName, opt => opt.MapFrom(src => src.Skill.SkillCategory.Name))
                .ReverseMap();
        }
    }
}