using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class GetUsersBySkillDTO
    {
        [JsonIgnore]
        [BindNever]
        public Guid SkillId { get; set; }
        public string? Purpose { get; set; }
    }
}