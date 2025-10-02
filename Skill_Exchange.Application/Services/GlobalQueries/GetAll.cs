using MediatR;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.GlobalQuery
{
    public record GetAll<T, TDTO> : IRequest<Result<IEnumerable<TDTO>>>
    where T : class
    where TDTO : class;

}