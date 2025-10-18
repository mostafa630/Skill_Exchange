using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public class AreFriendsHandler : IRequestHandler<AreFriends, Result<bool>>
    {
        private readonly UserManager<AppUser> _userManager;
        public AreFriendsHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<bool>> Handle(AreFriends request, CancellationToken cancellationToken)
        {
            try
            {
                var user1 = await _userManager.Users
                .Where(u => u.Id == request.user1Id)
                .Include(u => u.Friends)
                .Include(u => u.FriendOf)
                .FirstOrDefaultAsync(cancellationToken);

                if (user1 == null)
                    return Result<bool>.Fail("There exists a user that is not found");

                bool areFriends = user1.Friends.Any(f => f.Id == request.user2Id) ||
                              user1.FriendOf.Any(f => f.Id == request.user2Id);

                return Result<bool>.Ok(areFriends);
            }
            catch
            {
                return Result<bool>.Fail("checking Operation failed");
            }


        }
    }
}