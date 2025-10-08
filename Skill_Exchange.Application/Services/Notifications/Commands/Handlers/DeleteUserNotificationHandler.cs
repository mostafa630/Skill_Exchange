using MediatR;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.Notifications.Commands.Handlers
{
    public class DeleteSpecificUserNotificationHandler
        : IRequestHandler<DeleteSpecificUserNotification, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSpecificUserNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteSpecificUserNotification request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.GetRepository<AppUser>();
            var notificationRepo = _unitOfWork.GetRepository<Notification>();

            //  Get user by ID (without Include)
            var user = await userRepo.GetByIdAsync(request.userId);
            if (user == null)
                return Result<string>.Fail("User not found.");

            //  Get all notifications (since we can’t query with Include)
            var allNotifications = await notificationRepo.GetAllAsync();
            var notification = allNotifications.FirstOrDefault(n =>
                n.Id == request.notificationId &&
                n.Users.Any(u => u.Id == request.userId));

            if (notification == null)
                return Result<string>.Fail("Notification not found for this user.");

            //  Remove the relation manually
            notification.Users.Remove(user);

            //  Save changes
            var saved = await _unitOfWork.CompleteAsync();
            if (saved == 0)
                return Result<string>.Fail("Failed to delete the notification.");

            return Result<string>.Ok("Notification deleted successfully.");
        }
    }
}
