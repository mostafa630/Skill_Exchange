using MediatR;
using Skill_Exchange.Application.DTOs;
namespace Skill_Exchange.Application.Services.Notifications.Commands
{
    public record DeleteSpecificUserNotification(Guid userId, Guid notificationId)
        : IRequest<Result<string>>;
}
