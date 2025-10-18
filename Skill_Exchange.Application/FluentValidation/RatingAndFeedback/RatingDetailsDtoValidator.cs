using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class RatingDetailsDtoValidator : AbstractValidator<RatingDetailsDto>
    {
        public RatingDetailsDtoValidator()
        {
            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5)
                .When(x => x.Score.HasValue)
                .WithMessage("Score must be between 1 and 5.");

            RuleFor(x => x.Feedback)
                .MaximumLength(1000)
                .WithMessage("Feedback cannot exceed 1000 characters.");

            RuleFor(x => x.FromUserId)
                .NotEmpty()
                .WithMessage("FromUserId is required.");

            /*RuleFor(x => x.FromUserName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.FromUserName))
                .WithMessage("FromUserName cannot exceed 100 characters.");*/

            RuleFor(x => x.ToUserId)
                .NotEmpty()
                .WithMessage("ToUserId is required.");

            /*RuleFor(x => x.ToUserName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.ToUserName))
                .WithMessage("ToUserName cannot exceed 100 characters.");*/

            RuleFor(x => x.CreatedAt)
                .NotEqual(default(DateTime))
                .WithMessage("CreatedAt must be a valid date.");
        }
    }
}
