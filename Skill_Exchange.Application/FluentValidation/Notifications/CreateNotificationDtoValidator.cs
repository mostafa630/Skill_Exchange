using FluentValidation;
using Skill_Exchange.Application.DTOs.Notifications;
using System;

namespace Skill_Exchange.Application.Validators.Notifications
{
    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required.")
                .MaximumLength(500).WithMessage("Message must not exceed 500 characters.");

            RuleFor(x => x.RefrenceId)
                .NotEmpty().WithMessage("RefrenceId is required.");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("CreatedAt cannot be in the future.");
        }
    }
}
