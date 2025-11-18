using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Queries.Handlers
{
    public class GetRatingsReceivedByUserHandler
        : IRequestHandler<GetRatingsReceivedByUserQuery, Result<List<RatingReceivedByUserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRatingsReceivedByUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<RatingReceivedByUserDto>>> Handle(GetRatingsReceivedByUserQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _unitOfWork.RatingsAndFeedbacks
                .AsQueryable()
                .Include(r => r.FromUser)
                .Where(r => r.ToUserId == request.toUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(cancellationToken);

            // If no ratings → return empty list with Success = true
            if (!ratings.Any())
            {
                return Result<List<RatingReceivedByUserDto>>.Ok(new List<RatingReceivedByUserDto>());
            }

            var mapped = _mapper.Map<List<RatingReceivedByUserDto>>(ratings);

            foreach (var (dto, rating) in mapped.Zip(ratings, (dto, rating) => (dto, rating)))
            {
                dto.FromUserName = $"{rating.FromUser?.FirstName} {rating.FromUser?.LastName}".Trim();
            }

            return Result<List<RatingReceivedByUserDto>>.Ok(mapped);
        }

    }
}
