using MediatR;
using Skill_Exchange.Application.DTOs;
using System;

namespace Skill_Exchange.Application.Services.Notifications.Commands
{
    public record AddNotification(Guid UserId, string Title, string Message, Guid ReferenceId = default)
        : IRequest<Result<string>>;
}
