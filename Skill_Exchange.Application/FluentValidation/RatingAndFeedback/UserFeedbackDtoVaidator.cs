using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class UserFeedbackDtoValidator : AbstractValidator<UserFeedbackDto>
    {
        public UserFeedbackDtoValidator()
        {
            RuleFor(x => x.FromUserName)
                .NotEmpty().WithMessage("FromUserName is required.")
                .MaximumLength(100).WithMessage("FromUserName cannot exceed 100 characters.");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 5)
                .When(x => x.Score.HasValue)
                .WithMessage("Score must be between 0 and 5 if provided.");

            RuleFor(x => x.Feedback)
                .MaximumLength(500).WithMessage("Feedback cannot exceed 500 characters.");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("CreatedAt cannot be a future date.");
        }
    }
}
