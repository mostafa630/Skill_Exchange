using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries.Handlers
{
    public class GetRatingsGivenByUserHandler
        : IRequestHandler<GetRatingsGivenByUserQuery, Result<List<RatingGivenByUserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRatingsGivenByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<RatingGivenByUserDto>>> Handle(GetRatingsGivenByUserQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingsAndFeedbacks
                .AsQueryable()
                .Include(r => r.ToUser)
                .Where(r => r.FromUserId == request.fromUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);

            // If user gave no ratings → return empty list
            if (!ratings.Any())
            {
                return Result<List<RatingGivenByUserDto>>.Ok(new List<RatingGivenByUserDto>());
            }

            var mapped = _mapper.Map<List<RatingGivenByUserDto>>(ratings);

            foreach (var (dto, rating) in mapped.Zip(ratings, (dto, rating) => (dto, rating)))
            {
                dto.ToUserName = $"{rating.ToUser?.FirstName} {rating.ToUser?.LastName}".Trim();
            }

            return Result<List<RatingGivenByUserDto>>.Ok(mapped);
        }

    }
}
