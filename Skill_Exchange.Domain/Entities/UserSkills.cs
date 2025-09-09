using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;
namespace Skill_Exchange.Domain.Entities
{
    public class UserSkills
    {
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public ExchangePurpose Purpose { get; set; }

        //Navigation properties
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }

        public AppUser User { get; set; }
        public Skill Skill { get; set; }
    }
}