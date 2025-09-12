using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Skill_Exchange.Application.Services.GlobalQuery
{
    public record GetAll<T, E> : IRequest<IEnumerable<E>>;
}