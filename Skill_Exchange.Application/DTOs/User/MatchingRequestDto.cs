namespace Skill_Exchange.Application.DTOs.User
{
    public class MatchingRequestDTO
    {
        public List<string>? SkillsToLearn { get; set; }
        public int Top { get; set; } = 20;
    }
}
