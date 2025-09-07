using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;
namespace Skill_Exchange.Domain.Entities
{
    public class UserSkills
    {
        public Guid Id { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public ExchangePurpose Purpose { get; set; }
    }
}