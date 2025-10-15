using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Microsoft.AspNetCore.Http;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;

namespace Skill_Exchange.Application.Services.Request.Commands
{
    public record ChangeRequestStatus(Guid user1Id, Guid user2Id, ChangeRequestStatusDTO changeRequestStatusDTO) : IRequest<Result<string>>;
}