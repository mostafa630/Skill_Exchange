using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.UserSkill.Commands.Handlers
{
    public class DeleteUserSkillHandler : IRequestHandler<DeleteUserSkill, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserSkillHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(DeleteUserSkill request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _unitOfWork.UserSkills.DeleteUserSkill(request.deleteUserSkillDTO.UserId, request.deleteUserSkillDTO.SkillId);
                return res ? Result<bool>.Ok(res) : Result<bool>.Fail("Deletion Operation Failed or Entery does not Exist");
            }
            catch
            {
                return Result<bool>.Fail("Deletion Operation Failed or Entery does not Exist");
            }

        }
    }
}