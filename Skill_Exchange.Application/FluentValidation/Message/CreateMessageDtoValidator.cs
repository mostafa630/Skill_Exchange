using FluentValidation;
using Skill_Exchange.Application.DTOs.Message;

namespace Skill_Exchange.Application.FluentValidation.Message
{
    public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDTO>
    {
        public CreateMessageDtoValidator()
        {
            RuleFor(x => x.SenderId)
                .NotEmpty().WithMessage("SenderId is required.");

            RuleFor(x => x.ReceiverId)
                .NotEmpty().WithMessage("ReceiverId is required.");

            RuleFor(x => x.ConversationId)
                .NotEmpty().WithMessage("ConversationId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Message content cannot be empty.")
                .MaximumLength(1000).WithMessage("Message content cannot exceed 1000 characters.");
        }
    }
}
