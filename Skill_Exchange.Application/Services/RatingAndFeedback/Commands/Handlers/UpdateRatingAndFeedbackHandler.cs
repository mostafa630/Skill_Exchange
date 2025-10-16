using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.RatingAndFeedback.Commands.Handlers
{
    public class UpdateRatingAndFeedbackHandler
        : IRequestHandler<UpdateRatingAndFeedback, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRatingAndFeedbackHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateRatingAndFeedback request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.RatingsAndFeedbacks;

            // Fetch entity by id
            var existing = await repo.GetByIdAsync(request.Id);
            if (existing == null)
                return Result<string>.Fail("Rating not found.");

            // Apply updates
            existing.Score = request.UpdateRatingDto.Score;
            existing.Feedback = request.UpdateRatingDto.Feedback;

            var updated = await repo.UpdateAsync(existing);
            if (!updated)
                return Result<string>.Fail("Failed to update rating.");

            await _unitOfWork.CompleteAsync();
            return Result<string>.Ok("Rating updated successfully.");
        }
    }
}
