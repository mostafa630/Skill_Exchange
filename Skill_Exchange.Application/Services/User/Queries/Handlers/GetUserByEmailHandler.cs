using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmail, Result<UserDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public GetUserByEmailHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(GetUserByEmail request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.email);
                if (user == null)
                {
                    return Result<UserDTO>.Fail("No user Exists");
                }
               
                return Result<UserDTO>.Ok(userDto);
            }
            catch
            {
                return Result<UserDTO>.Fail("Operation Failed");
            }
        }

    }
}