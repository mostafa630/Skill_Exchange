using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;

namespace Skill_Exchange.Application.Services.User.Queries
{
    public record GetAllUsers() : IRequest<Result<IEnumerable<UserDTO>>>;
}