using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.UserSkill;

namespace Skill_Exchange.Application.Services.UserSkill.Queries
{
    public record GetUsersBySkill(GetUsersBySkillDTO getUsersBySkillDTO) : IRequest<Result<IEnumerable<UserWithSkillInfoDto>>>;


}