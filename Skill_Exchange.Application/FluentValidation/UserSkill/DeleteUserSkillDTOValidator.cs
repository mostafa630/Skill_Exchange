using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Skill_Exchange.Application.DTOs.UserSkill;

namespace Skill_Exchange.Application.FluentValidation.UserSkill
{
    public class DeleteUserSkillDTOValidator : AbstractValidator<DeleteUserSkillDTO>
    {
        public DeleteUserSkillDTOValidator()
        {
            RuleFor(r => r.UserId)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must be a valid, non-empty GUID.");

            RuleFor(r => r.SkillId)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must be a valid, non-empty GUID.");
        }
    }
}