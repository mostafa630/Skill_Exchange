using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class UserWithSkillInfoDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int YearsOfExperience { get; set; }
        public string Description { get; set; }
        public ExchangePurpose Purpose { get; set; }
    }
}