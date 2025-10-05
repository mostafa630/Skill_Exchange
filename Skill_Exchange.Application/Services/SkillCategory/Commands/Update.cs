using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill_Category;
namespace Skill_Exchange.Application.Services.SkillCategory.Commands
{
    public record UpdateSkillCategory (SkillCategoryDTO updateSkillCategoryDTO) : IRequest<Result<string>>;

}
