using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.UserSkill.Queries.Handlers
{
    public class GetUserSkillsHandler : IRequestHandler<GetUserSkills, Result<IEnumerable<UserSkillDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserSkillsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserSkillDTO>>> Handle(GetUserSkills request, CancellationToken cancellationToken)
        {
            try
            {
                var userSkills = await _unitOfWork.UserSkills.GetUserSkillsAsync(request.getUserSkillsDTO.UserId, request.getUserSkillsDTO.Purpose?.ToString());
                var userSkillsDTos = _mapper.Map<IEnumerable<UserSkillDTO>>(userSkills);
                return Result<IEnumerable<UserSkillDTO>>.Ok(userSkillsDTos);
            }
            catch
            {
                return Result<IEnumerable<UserSkillDTO>>.Fail("Operation Failed");
            }
        }
    }
}