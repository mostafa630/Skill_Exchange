using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.GlobalQuery
{
    public record GetAll<T, TDTO>(ISpecification<T> spec) : IRequest<Result<IEnumerable<TDTO>>>
    where T : class
    where TDTO : class;

}