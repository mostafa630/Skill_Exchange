namespace Skill_Exchange.Application.DTOs.Skill
{
    public class SkillResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPredefined { get; set; }
        public string CreatedBy { get; set; } 
        public Guid SkillCategoryId { get; set; }

    }
}
