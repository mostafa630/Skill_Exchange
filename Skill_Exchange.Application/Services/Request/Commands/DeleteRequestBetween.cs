using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;

namespace Skill_Exchange.Application.Services.Request.Commands
{
    public record DeleteRequestBetween(BetweenDto delteRequestBetweenDTO) : IRequest<Result<bool>>;
}