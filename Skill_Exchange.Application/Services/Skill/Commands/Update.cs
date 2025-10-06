using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill;
namespace Skill_Exchange.Application.Services.Skill.Commands
{
    public record UpdateSkill(UpdateSkillDto updateSkillDTO) : IRequest<Result<string>>;
}
