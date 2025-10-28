using Skill_Exchange.Application.DTOs.Skill;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UserMatchDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public double MatchScore { get; set; }
        public string? ImageUrl { get; set; }
        public string Bio { get; set; }

        public List<SkillDTO> SkillsToTeach { get; set; } = new();
        public List<SkillDTO> SkillsToLearn { get; set; } = new();
    }
}
