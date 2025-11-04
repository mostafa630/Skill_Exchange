using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.RatingAndFeedback;

namespace Skill_Exchange.Application.Services.Conversation.Queries
{
    public record GetConversation(Guid User1Id,Guid User2Id) : IRequest<Result<Guid>>;
}
