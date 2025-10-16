using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class UpdateRatingDtoValidator : AbstractValidator<UpdateRatingDto>
    {
        public UpdateRatingDtoValidator() 
        {
            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5).When(x => x.Score.HasValue)
                .WithMessage("Score must be between 1 and 5.");
            RuleFor(x => x.Feedback)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Feedback))
                .WithMessage("Feedback cannot exceed 1000 characters.");
        }
    }
}
