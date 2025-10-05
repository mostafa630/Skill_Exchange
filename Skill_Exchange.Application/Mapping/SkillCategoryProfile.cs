using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Domain.Entities;
using AutoMapper;
namespace Skill_Exchange.Application.Mapping
{
    public class SkillCategoryProfile: Profile
    {
        public SkillCategoryProfile()
        {
            // DTO -> Entity
            CreateMap<SkillCategoryDTO, SkillCategory>().ReverseMap();
        }
    }
}
