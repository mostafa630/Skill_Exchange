using MediatR;
using Skill_Exchange.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.GlobalQuery
{
    public record GetById<T, TDTO>(Guid Id) : IRequest<Result<TDTO>>
    where T : class
    where TDTO : class;

}
