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
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, IEnumerable<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<IEnumerable<UserDTO>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = _unitOfWork.Users.GetAllAsync();
            var usersDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Task.FromResult(usersDtos);
        }
    }
}