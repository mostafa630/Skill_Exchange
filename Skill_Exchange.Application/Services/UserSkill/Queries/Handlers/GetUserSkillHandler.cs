using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.UserSkill.Queries.Handlers
{
    public class GetUserSkillHandler : IRequestHandler<GetUserSkill, Result<UserSkillDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserSkillHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<UserSkillDTO>> Handle(GetUserSkill request, CancellationToken cancellationToken)
        {
            try
            {

                var userSkill = await _unitOfWork.UserSkills.GetUserSkillAsync(request.getUserSkillDTO.UserId, request.getUserSkillDTO.SkillId);

                if (userSkill is null)
                {
                    return Result<UserSkillDTO>.Fail("User doesn't has this Skill");
                }

                var userSkillDTO = _mapper.Map<UserSkillDTO>(userSkill);

                return Result<UserSkillDTO>.Ok(userSkillDTO);
            }
            catch
            {
                return Result<UserSkillDTO>.Fail("Operation failed");
            }
        }
    }
}