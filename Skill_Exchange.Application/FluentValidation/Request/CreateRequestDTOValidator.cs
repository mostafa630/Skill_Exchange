using FluentValidation;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.FluentValidation.Request
{
    public class CreateRequestDTOValidator : AbstractValidator<CreateRequestDTO>
    {
        public CreateRequestDTOValidator()
        {
            RuleFor(r => r.Status)
            .IsInEnum()
            .WithMessage($"Status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(RequestStatus)))}");


            RuleFor(r => r.CreatedAt)
            .Must(date => date != default(DateTime))
            .WithMessage("CreatedAt must be a valid date.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("CreatedAt cannot be in the future.");

            RuleFor(r => r.RespondedAt)
            .Must(date => date != default(DateTime))
            .WithMessage("CreatedAt must be a valid date.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("CreatedAt cannot be in the future.");

            RuleFor(r => r.SenderId)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("SenderId must be a valid, non-empty GUID.")
            .Must((r, senderId) => senderId != r.RecieverId)
            .WithMessage("SenderId and RecieverId cannot be the same.");

            RuleFor(r => r.RecieverId)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("SenderId must be a valid, non-empty GUID.");

            // RuleFor(r => r)
            // .Must(r => r.SenderId != r.RecieverId)
            // .WithMessage("SenderId and RecieverId cannot be the same.");

        }
    }
}