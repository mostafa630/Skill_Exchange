using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.Skill.Commands.Handlers
{
    public class UpdateSkillHandler : IRequestHandler<UpdateSkill, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSkillHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateSkill request, CancellationToken cancellationToken)
        {
            var dto = request.updateSkillDTO;

            var existingSkill = await _unitOfWork.Skills.GetByIdAsync(dto.Id);
            if (existingSkill == null)
                return Result<string>.Fail("Skill not found.");

            existingSkill.Name = dto.Name;
            existingSkill.IsPredefined = dto.IsPredefined;
            existingSkill.SkillCategoryId = dto.SkillCategoryId;

            await _unitOfWork.Skills.UpdateAsync(existingSkill);

            var result = await _unitOfWork.CompleteAsync();
            if (result == 0)
                return Result<string>.Fail("Failed to update skill.");

            return Result<string>.Ok("Skill updated successfully.");
        }
    }
}
