using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class UserSkillsDTO
    {

        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }
        public int yearsOfExperience { get; set; }
        public string Description { get; set; }
        public ExchangePurpose Purpose { get; set; }
    }

}