using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.User.Commands
{
    public record DelteUser(string email) : IRequest<Result<bool>>;
}