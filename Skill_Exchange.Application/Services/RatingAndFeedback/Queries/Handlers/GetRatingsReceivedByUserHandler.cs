using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries.Handlers
{
    public class GetRatingsReceivedByUserHandler
        : IRequestHandler<GetRatingsReceivedByUserQuery, Result<List<UserRatingsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRatingsReceivedByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<UserRatingsDto>>> Handle(GetRatingsReceivedByUserQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingsAndFeedbacks.GetAllAsync();

            var receivedRatings = ratings
                .Where(r => r.ToUserId == request.toUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            if (!receivedRatings.Any())
                return Result<List<UserRatingsDto>>.Fail("No ratings found for this user.");

            var mapped = _mapper.Map<List<UserRatingsDto>>(receivedRatings);
            return Result<List<UserRatingsDto>>.Ok(mapped);
        }
    }
}
