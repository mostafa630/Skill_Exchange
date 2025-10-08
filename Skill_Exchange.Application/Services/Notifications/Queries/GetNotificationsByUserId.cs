using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.Notifications.Queries
{
    public record GetNotificationsByUserId(Guid userId) : IRequest<Result<List<NotificationDto>>>;
}
