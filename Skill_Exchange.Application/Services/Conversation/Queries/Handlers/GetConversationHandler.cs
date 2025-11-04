using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Conversation.Queries.Handlers
{
    public class GetConversationHandler : IRequestHandler<GetConversation, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetConversationHandler(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }
        public async Task<Result<Guid>> Handle(GetConversation request, CancellationToken cancellationToken)
        {
            var convserationId = await _unitOfWork.Conversations.GetConversationIdBetween(request.User1Id, request.User2Id);
            if (convserationId == Guid.Empty) 
                return Result<Guid>.Fail("There is no convrsation between those uers");
            return Result<Guid>.Ok(convserationId);
        }
    }
}
