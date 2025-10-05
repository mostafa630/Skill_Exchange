using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Services.User.Commands.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DelteUser, Result<bool>>
    {
        public readonly UserManager<AppUser> _userManager;
        public DeleteUserHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DelteUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.email);
                if (user == null)
                {
                    return Result<bool>.Fail("No user Exists");
                }
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return Result<bool>.Fail("Operation Failed");
                }
                return Result<bool>.Ok(true);
            }
            catch
            {
                return Result<bool>.Fail("Operation Failed");
            }
        }
    }
}