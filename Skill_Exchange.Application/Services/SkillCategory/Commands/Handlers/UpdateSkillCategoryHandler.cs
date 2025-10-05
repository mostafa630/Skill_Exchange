using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill_Category;
using Skill_Exchange.Application.Services.SkillCategory.Commands;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.SkillCategory.Commands.Handlers
{
    public class UpdateSkillCategoryHandler : IRequestHandler<UpdateSkillCategory, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSkillCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateSkillCategory request, CancellationToken cancellationToken)
        {
            var dto = request.updateSkillCategoryDTO;

            // Get the entity by ID
            var skillCategory = await _unitOfWork.SkillCategories.GetByIdAsync(dto.Id);

            if (skillCategory == null)
                return Result<string>.Fail("Skill category not found.");

            // Update properties
            skillCategory.Name = dto.Name;
            skillCategory.Description = dto.Description;

            await _unitOfWork.SkillCategories.UpdateAsync(skillCategory);
            await _unitOfWork.CompleteAsync();

            return Result<string>.Ok("Skill category updated successfully.");
        }
    }
}
