using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Application.Services.RatingAndFeedback.Queries;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Handlers
{
    public class GetUserRatingSummaryHandler
        : IRequestHandler<GetUserRatingSummary, Result<UserRatingSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserRatingSummaryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserRatingSummaryDto>> Handle(GetUserRatingSummary request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingsAndFeedbacks
                .AsQueryable()
                .Include(r => r.FromUser)
                .Where(r => r.ToUserId == request.UserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);

            // If user has NO ratings → return an EMPTY summary (Success = true)
            if (!ratings.Any())
            {
                var emptySummary = new UserRatingSummaryDto
                {
                    UserId = request.UserId,
                    AverageScore = 0,
                    TotalRatings = 0,
                    Feedbacks = new List<UserFeedbackDto>()
                };

                return Result<UserRatingSummaryDto>.Ok(emptySummary);
            }

            // Normal case: user has ratings
            var feedbacks = _mapper.Map<List<UserFeedbackDto>>(ratings);
            var summary = _mapper.Map<UserRatingSummaryDto>(ratings);

            summary.UserId = request.UserId;
            summary.Feedbacks = feedbacks;
            summary.TotalRatings = ratings.Count;
            summary.AverageScore = Math.Round(
                ratings.Where(r => r.Score.HasValue).Average(r => r.Score!.Value), 2
            );

            return Result<UserRatingSummaryDto>.Ok(summary);
        }

    }
}
