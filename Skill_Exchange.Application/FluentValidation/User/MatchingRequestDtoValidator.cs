using FluentValidation;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.FluentValidation.User
{
    public class MatchingRequestDtoValidator : AbstractValidator<MatchingRequestDTO>
    {
        public MatchingRequestDtoValidator()
        {
            // 'Top' is optional, but must be reasonable if provided
            RuleFor(x => x.Top)
                .GreaterThan(0).WithMessage("Top must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Top cannot exceed 100.");

            // If skills are provided, validate them
            When(x => x.SkillsToLearn != null && x.SkillsToLearn.Any(), () =>
            {
                RuleForEach(x => x.SkillsToLearn)
                    .NotEmpty().WithMessage("Skill name cannot be empty.")
                    .MaximumLength(100).WithMessage("Skill name cannot exceed 100 characters.");
            });
        }
    }
}
