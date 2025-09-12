using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.User.Queries
{
    public record GetUserById(Guid id) : IRequest<UserDTO>;
}