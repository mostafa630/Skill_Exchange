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
    public class DeleteUserNotificationHandler : IRequestHandler<DeleteUserNotification, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(DeleteUserNotification request, CancellationToken cancellationToken)
        {
            var userRepo = _unitOfWork.GetRepository<AppUser>();

            // Get all users
            var users = await userRepo.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Id == request.userId);

            if (user == null)
                return Result<string>.Fail("User not found.");

            if (user.Notifications == null || !user.Notifications.Any())
                return Result<string>.Fail("No notifications found for this user.");

            // Clear notifications
            user.Notifications.Clear();

            // Save changes
            var saved = await _unitOfWork.CompleteAsync();
            if (saved == 0)
                return Result<string>.Fail("Failed to delete notifications for the user.");

            return Result<string>.Ok("All user notifications deleted successfully.");
        }
    }
}
