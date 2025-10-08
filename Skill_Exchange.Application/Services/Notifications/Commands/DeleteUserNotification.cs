using MediatR;
using Skill_Exchange.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.Notifications.Commands
{
    public record DeleteUserNotification(Guid userId) : IRequest<Result<string>>;
}
