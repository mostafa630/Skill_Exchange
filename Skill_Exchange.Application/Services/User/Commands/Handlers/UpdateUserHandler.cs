using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Services.User.Commands.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser, Result<UserDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public UpdateUserHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Result<UserDTO>> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.updateUserDTO.Email);
                if (user == null)
                {
                    return Result<UserDTO>.Fail("No user Exists");
                }

                user.FirstName = request.updateUserDTO.FirstName ?? user.FirstName;
                user.LastName = request.updateUserDTO.LastName ?? user.LastName;
                user.DateOfBirth = request.updateUserDTO.DateOfBirth ?? user.DateOfBirth;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Result<UserDTO>.Fail("Operation Failed");
                }
                var userDto = _mapper.Map<UserDTO>(user);
                return Result<UserDTO>.Ok(userDto);

            }
            catch
            {
                return Result<UserDTO>.Fail("Operation Failed");
            }

        }
    }
}