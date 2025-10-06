﻿using FluentValidation;
using Skill_Exchange.Application.DTOs.Skill_Category;
namespace Skill_Exchange.Application.FluentValidation.SkillCategory
{
    public class SkillCategoryDTOValidator: AbstractValidator<SkillCategoryDTO>
    {
        public SkillCategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
