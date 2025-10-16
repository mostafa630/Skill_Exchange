using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries
{
    public record GetRatingsGivenByUserQuery(Guid fromUserId): IRequest<Result<List<UserRatingsDto>>>;
}
