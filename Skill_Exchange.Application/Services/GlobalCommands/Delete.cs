using MediatR;
using Skill_Exchange.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.GlobalCommands
{
    public record Delete<T>(Guid Id) : IRequest<Result<string>> where T : class;
}
