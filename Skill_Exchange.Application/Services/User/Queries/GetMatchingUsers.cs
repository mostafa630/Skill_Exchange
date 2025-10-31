using MediatR;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.Users.Queries
{
    public record GetMatchingUsers(Guid UserId, List<Guid>? SkillsToLearn = null, PaginationDto Pagination = null)
        : IRequest<Result<List<UserMatchDTO>>>;
}
