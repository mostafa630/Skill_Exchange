using MediatR;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.Services.Users.Queries
{
    public record GetMatchingUsers(Guid UserId, List<string>? SkillsToLearn = null, int Top = 20)
        : IRequest<List<UserMatchDTO>>;
}
