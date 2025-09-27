using FluentValidation;
using Skill_Exchange.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class StartRegisterRequestDtoValidator: AbstractValidator<StartRegisterRequestDto>
    {
        public StartRegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
