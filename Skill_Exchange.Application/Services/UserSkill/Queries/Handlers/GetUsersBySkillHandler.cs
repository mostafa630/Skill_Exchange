using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.UserSkill.Queries.Handlers
{
    public class GetUsersBySkillHandler : IRequestHandler<GetUsersBySkill, Result<IEnumerable<UserDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUsersBySkillHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserDTO>>> Handle(GetUsersBySkill request, CancellationToken cancellationToken)
        {
            try
            {
                var users = _unitOfWork.UserSkills.GetUsersBySkillAsync(request.getUsersBySkillDTO.SkillId, request.getUsersBySkillDTO.Purpose?.ToString());
                var usersDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
                return Result<IEnumerable<UserDTO>>.Ok(usersDTOs);
            }
            catch
            {
                return Result<IEnumerable<UserDTO>>.Fail("Operation Failed");
            }


        }
    }
}