using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class GetUserSkillsDTO
    {
        public Guid UserId { get; set; }
        public string? Purpose { get; set; }
    }
}