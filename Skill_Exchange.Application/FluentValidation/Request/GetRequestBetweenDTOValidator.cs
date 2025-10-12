using FluentValidation;
using Skill_Exchange.Application.DTOs.Request;

namespace Skill_Exchange.Application.FluentValidation.Request
{
    public class GetRequestBetweenDTOValidator : AbstractValidator<BetweenDto>
    {
        public GetRequestBetweenDTOValidator()
        {
            RuleFor(r => r.User1Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("SenderId must be a valid, non-empty GUID.");

            RuleFor(r => r.User2Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("SenderId must be a valid, non-empty GUID.");
        }
    }
}