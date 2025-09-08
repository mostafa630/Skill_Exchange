using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPredefined { get; set; }
        public string CreatedBy { get; set; }

        //Navigation properties
        public Guid SkillCategoryId { get; set; }
        public SkillCategory SkillCategory { get; set; }             
        public ICollection<AppUser> Users { get; set; }
    }
}