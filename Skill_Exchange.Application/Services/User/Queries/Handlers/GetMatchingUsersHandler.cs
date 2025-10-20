using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.User.Queries.Handlers
{
    public record GetMatchingUsers(string email) : IRequest<Result<UserDTO>>;
}
