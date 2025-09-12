using MediatR;

namespace Skill_Exchange.Application.Services.GlobalQuery
{
    public record GetAll<T, TDTO> : IRequest<IEnumerable<TDTO>>
    where T : class
    where TDTO : class;

}