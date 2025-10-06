namespace Skill_Exchange.Application.DTOs.Skill
{
    public class CreateSkillDto
    {
        public string Name { get; set; }
        public bool IsPredefined { get; set; } = false;
        public Guid SkillCategoryId { get; set; }
    }
}
