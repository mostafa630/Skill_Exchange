using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<UserDTO> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.Users.GetByIdAsync(request.id);
            var userDto = _mapper.Map<UserDTO>(user);
            return Task.FromResult(userDto);
        }
    }
}