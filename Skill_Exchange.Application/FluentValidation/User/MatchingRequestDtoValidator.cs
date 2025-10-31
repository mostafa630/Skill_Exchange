using FluentValidation;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.FluentValidation.User
{
    public class MatchingRequestDtoValidator : AbstractValidator<MatchingRequestDTO>
    {
        public MatchingRequestDtoValidator()
        {
            // 'SkillsToLearn' is optional, but if provided, must not be empty and contain valid GUIDs
            RuleFor(x => x.SkillsToLearn)
                .Must(list => list == null || list.Count > 0)
                .WithMessage("SkillsToLearn cannot be an empty list if provided.");

        }
    }
}
