using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class UserRatingSummaryDtoValidator : AbstractValidator<UserRatingSummaryDto>
    {
        public UserRatingSummaryDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.AverageScore)
                .InclusiveBetween(0, 5).WithMessage("AverageScore must be between 0 and 5.");

            RuleFor(x => x.TotalRatings)
                .GreaterThanOrEqualTo(0).WithMessage("TotalRatings cannot be negative.");

            RuleForEach(x => x.Feedbacks)
                .SetValidator(new UserFeedbackDtoValidator());
        }
    }
}
