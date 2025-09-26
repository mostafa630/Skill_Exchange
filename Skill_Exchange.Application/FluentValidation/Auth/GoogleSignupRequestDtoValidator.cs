﻿using FluentValidation;
using Skill_Exchange.Application.DTOs.Auth;
namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class GoogleSignupRequestDtoValidator : AbstractValidator<GoogleSignupRequestDto>
    {
        public GoogleSignupRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("IdToken is required.")
                .MinimumLength(20).WithMessage("IdToken must be at least 20 characters long.");
        }
    }
}
