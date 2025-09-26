﻿using FluentValidation;
using Skill_Exchange.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.FluentValidation.Auth
{
    public class ResetPasswordRequestDtoValidator: AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator() 
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Reset token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("New password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character.");

        }
    }
}
