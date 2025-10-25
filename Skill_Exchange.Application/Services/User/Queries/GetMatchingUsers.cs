using MediatR;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.Users.Queries
{
    public record GetMatchingUsers(Guid UserId, List<Guid>? SkillsToLearn = null, int Top = 20)
        : IRequest<Result<List<UserMatchDTO>>>;
}
