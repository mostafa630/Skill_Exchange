
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Skill_Exchange.Application.DTOs.UserSkill
{
    public class GetUserSkillsDTO
    {
        [JsonIgnore]
        [BindNever]
        public Guid UserId { get; set; } = default;
        public string? Purpose { get; set; }
    }
}