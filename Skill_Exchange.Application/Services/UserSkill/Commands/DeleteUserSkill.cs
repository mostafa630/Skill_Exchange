using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.UserSkill;

namespace Skill_Exchange.Application.Services.UserSkill.Commands
{
    public record DeleteUserSkill(DeleteUserSkillDTO deleteUserSkillDTO) : IRequest<Result<bool>>;

}