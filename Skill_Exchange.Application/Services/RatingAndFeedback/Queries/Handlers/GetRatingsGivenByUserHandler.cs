using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries.Handlers
{
    public class GetRatingsGivenByUserHandler
        : IRequestHandler<GetRatingsGivenByUserQuery, Result<List<UserRatingsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRatingsGivenByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<UserRatingsDto>>> Handle(GetRatingsGivenByUserQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingsAndFeedbacks.GetAllAsync();

            var givenRatings = ratings
                .Where(r => r.FromUserId == request.fromUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            if (!givenRatings.Any())
                return Result<List<UserRatingsDto>>.Fail("No ratings found from this user.");

            var mapped = _mapper.Map<List<UserRatingsDto>>(givenRatings);
            return Result<List<UserRatingsDto>>.Ok(mapped);
        }
    }
}
