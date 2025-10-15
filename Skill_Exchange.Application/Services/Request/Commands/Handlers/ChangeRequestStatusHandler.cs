
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Request.Commands.Handlers
{
    public class ChangeRequestStatusHandler : IRequestHandler<ChangeRequestStatus, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChangeRequestStatusHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(ChangeRequestStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var _request = await _unitOfWork.Requests.GetRequestBetweenAsync(request.user1Id, request.user2Id);
                if (_request is null)
                {
                    return Result<string>.Fail("There is no request between users");
                }
                _request.Status = request.changeRequestStatusDTO.Status;
                _request.RespondedAt = DateTime.Now;
                await _unitOfWork.CompleteAsync();
                return Result<string>.Ok("Updating Status Done");
            }
            catch
            {
                return Result<string>.Fail("Updating Status Failed");
            }
        }
    }
}