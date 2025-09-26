using Skill_Exchange.Application.DTOs.Auth;
using FluentValidation;
namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class GoogleLoginRequestDtoValidator: AbstractValidator<GoogleLoginRequestDto>
    {
        public GoogleLoginRequestDtoValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("IdToken is required.")
                .MinimumLength(20).WithMessage("IdToken must be at least 20 characters long.");
        }

    }
}
