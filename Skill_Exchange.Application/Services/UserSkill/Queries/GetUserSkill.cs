using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.UserSkill;

namespace Skill_Exchange.Application.Services.UserSkill.Queries
{
    public record GetUserSkill(GetUserSkillDTO getUserSkillDTO) : IRequest<Result<UserSkillDTO>>;

}