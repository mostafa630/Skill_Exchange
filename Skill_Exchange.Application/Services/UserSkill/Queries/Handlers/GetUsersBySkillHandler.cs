using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.UserSkill;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.UserSkill.Queries.Handlers
{
    public class GetUsersBySkillHandler : IRequestHandler<GetUsersBySkill, Result<IEnumerable<UserWithSkillInfoDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUsersBySkillHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserWithSkillInfoDto>>> Handle(GetUsersBySkill request, CancellationToken cancellationToken)
        {
            try
            {
                var userSkills = await _unitOfWork.UserSkills.GetUsersBySkillAsync(request.getUsersBySkillDTO.SkillId, request.getUsersBySkillDTO.Purpose?.ToString());
                var skillUsersInfo = _mapper.Map<IEnumerable<UserWithSkillInfoDto>>(userSkills)
                .DistinctBy(u => u.Id) // avoids duplicates if any
                .ToList();

                return Result<IEnumerable<UserWithSkillInfoDto>>.Ok(skillUsersInfo);
            }
            catch
            {
                return Result<IEnumerable<UserWithSkillInfoDto>>.Fail("Operation Failed");
            }
        }
    }
}