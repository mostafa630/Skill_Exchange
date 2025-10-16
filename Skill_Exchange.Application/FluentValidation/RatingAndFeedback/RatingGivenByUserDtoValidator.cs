using FluentValidation;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using System;

namespace Skill_Exchange.Application.FluentValidation.RatingAndFeedback
{
    public class RatingGivenByUserDtoValidator : AbstractValidator<RatingGivenByUserDto>
    {
        public RatingGivenByUserDtoValidator()
        {
            RuleFor(x => x.Score)
                .NotNull().WithMessage("Score is required.")
                .InclusiveBetween(1, 5).WithMessage("Score must be between 1 and 5.");

            RuleFor(x => x.Feedback)
                .MaximumLength(500).WithMessage("Feedback cannot exceed 500 characters.");

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage("CreatedAt date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

            RuleFor(x => x.ToUserId)
                .NotEmpty().WithMessage("ToUserId (receiver) is required.");

            RuleFor(x => x.ToUserName)
                .MaximumLength(100).WithMessage("ToUserName cannot exceed 100 characters.");
        }
    }
}
