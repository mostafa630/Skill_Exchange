using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsers, Result<IEnumerable<UserDTO>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<UserDTO>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var usersDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
                return Result<IEnumerable<UserDTO>>.Ok(usersDtos);
            }
            catch
            {
                return Result<IEnumerable<UserDTO>>.Fail("Operation Failed");
            }
        }
    }
}