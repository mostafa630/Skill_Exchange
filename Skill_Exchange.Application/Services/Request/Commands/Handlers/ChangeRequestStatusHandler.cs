
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Enums;
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
                _request.RespondedAt = DateTime.UtcNow;
                if (_request.Status == RequestStatus.Accepted)
                {
                    await AddFriend(_request.SenderId, _request.RecieverId);
                }
                await _unitOfWork.CompleteAsync();
                return Result<string>.Ok("Updating Status Done");
            }
            catch
            {
                return Result<string>.Fail("Updating Status Failed");
            }
        }
        private async Task AddFriend(Guid senderId, Guid recieverId)
        {
            try
            {
                var sender = await _unitOfWork.Users.GetByIdAsync(senderId);
                var reciever = await _unitOfWork.Users.GetByIdAsync(recieverId);
                sender.Friends.Add(reciever);
                reciever.FriendOf.Add(sender);
            }
            catch
            {
                throw new Exception("Add Friend Operation failed");
            }
        }
    }


}