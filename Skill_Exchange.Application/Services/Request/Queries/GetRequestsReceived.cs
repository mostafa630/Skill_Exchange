using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;

namespace Skill_Exchange.Application.Services.Request.Queries
{
    public record GetRequestsReceived(Guid RecieverId) : IRequest<Result<IEnumerable<RequestDTO>>>;
}