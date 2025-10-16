using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class AddRatingAndFeedbackDtoValidator: AbstractValidator<AddRatingAndFeedbackDto>
    {
        public AddRatingAndFeedbackDtoValidator()
        {
            RuleFor(x => x.ToUserId)
                .NotEmpty().WithMessage("ToUserId is required.")
                .NotEqual(Guid.Empty).WithMessage("ToUserId cannot be an empty GUID.");
            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5).When(x => x.Score.HasValue)
                .WithMessage("Score must be between 1 and 5 if provided.");
            RuleFor(x => x.Feedback)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Feedback))
                .WithMessage("Feedback cannot exceed 1000 characters.");
        }

    }
}
