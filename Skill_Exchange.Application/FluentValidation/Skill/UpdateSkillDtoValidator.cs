using FluentValidation;
using Skill_Exchange.Application.DTOs.Skill;

namespace Skill_Exchange.Application.FluentValidation.Skill
{
    public class UpdateSkillDtoValidator : AbstractValidator<UpdateSkillDto>
    {
        public UpdateSkillDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Skill ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid Skill ID.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required.")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters.");

            RuleFor(x => x.SkillCategoryId)
                .NotEmpty().WithMessage("Skill category ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Invalid SkillCategoryId value.");
        }
    }
}
