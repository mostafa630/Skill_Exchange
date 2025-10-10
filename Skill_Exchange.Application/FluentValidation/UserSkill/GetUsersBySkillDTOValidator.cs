using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.FluentValidation.UserSkill
{
    public class GetUsersBySkillDTOValidator : AbstractValidator<GetUsersBySkillDTO>
    {
        public GetUsersBySkillDTOValidator()
        {
            RuleFor(r => r.SkillId)
               .NotEmpty()
               .Must(id => id != Guid.Empty)
               .WithMessage("Id must be a valid, non-empty GUID.");

            RuleFor(r => r.Purpose)
             .Must(value => string.IsNullOrEmpty(value) || Enum.TryParse<ExchangePurpose>(value, true, out _))
             .WithMessage($"Purpose must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(ExchangePurpose)))}");
        }
    }
}