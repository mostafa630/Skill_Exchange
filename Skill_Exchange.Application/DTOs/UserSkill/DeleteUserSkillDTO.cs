using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class DeleteUserSkillDTO
    {
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }

    }
}