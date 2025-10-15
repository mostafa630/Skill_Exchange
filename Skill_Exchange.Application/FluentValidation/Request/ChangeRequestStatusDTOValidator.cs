using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.FluentValidation.Request
{
    public class ChangeRequestStatusDTOValidator : AbstractValidator<ChangeRequestStatusDTO>
    {
        public ChangeRequestStatusDTOValidator()
        {
            RuleFor(r => r.Status)
            .IsInEnum()
            .WithMessage($"Status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(RequestStatus)))}");

        }
    }
}