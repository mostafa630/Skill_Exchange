using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Commands
{
    public record UpdateRatingAndFeedback(Guid Id, UpdateRatingDto UpdateRatingDto): IRequest<Result<string>>;
}
