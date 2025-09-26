using FluentValidation;
using Skill_Exchange.Application.DTOs.Auth;
namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class ConfirmEmailRequestDtoValidator:AbstractValidator<ConfirmEmailRequestDto>
    {
        public ConfirmEmailRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User Id is required.");
        }
    }
}
