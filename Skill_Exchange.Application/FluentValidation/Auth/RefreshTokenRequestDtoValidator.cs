﻿using FluentValidation;
using Skill_Exchange.Application.DTOs.Auth;
namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class RefreshTokenRequestDtoValidator: AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required.")
                .MinimumLength(20).WithMessage("Access token must be at least 20 characters long.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(20).WithMessage("Refresh token must be at least 20 characters long.");

        }
    }
}
