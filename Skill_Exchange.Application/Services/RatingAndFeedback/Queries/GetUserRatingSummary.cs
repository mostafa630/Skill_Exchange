using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.Services.RatingAndFeedback
{
    public record GetUserRatingSummary(Guid UserId) : IRequest<Result<UserRatingSummaryDto>>;
}
