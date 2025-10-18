using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Skill_Exchange.Application.DTOs;

namespace Skill_Exchange.Application.Services.User.Queries
{
    public record AreFriends(Guid user1Id, Guid user2Id) : IRequest<Result<bool>>;

}