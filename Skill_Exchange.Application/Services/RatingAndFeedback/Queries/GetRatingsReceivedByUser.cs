using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using System;
using System.Collections.Generic;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries
{
    public record GetRatingsReceivedByUserQuery(Guid toUserId): IRequest<Result<List<UserRatingsDto>>>;
}
