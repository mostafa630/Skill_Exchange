using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmail, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(GetUserByEmail request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.email);
            if (user == null)
            {
                return null;
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return userDto;
        }
    }
}