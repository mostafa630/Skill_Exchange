using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class GetUsersBySkillDTO
    {
        public Guid SkillId { get; set; }
        public string? Purpose { get; set; }
    }
}