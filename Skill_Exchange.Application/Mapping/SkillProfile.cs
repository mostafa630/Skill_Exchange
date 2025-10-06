using AutoMapper;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Application.DTOs.Skill;

namespace Skill_Exchange.Application.Mapping
{
    public class SkillProfile:Profile
    {
        public SkillProfile()
        {
            CreateMap<Skill, CreateSkillDto>().ReverseMap();
            CreateMap<Skill, UpdateSkillDto>().ReverseMap();
            CreateMap<Skill, SkillResponseDto>().ReverseMap();
        }

    }
}
