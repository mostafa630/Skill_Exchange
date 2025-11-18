using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.Services.Notifications.Commands;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Notifications.Commands.Handlers
{
    public class AddNotificationHandler : IRequestHandler<AddNotification, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(AddNotification request, CancellationToken cancellationToken)
        {
            // 1️ Validate user exists
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Fail("User not found.");

            // 2️ Create Notification entity
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow,
                RefrenceId = request.ReferenceId, 
                Users = new List<AppUser> { user }
            };

            // 3️ Save notification
            var success = await _unitOfWork.Notifications.AddAsync(notification);
            if (!success)
                return Result<string>.Fail("Failed to store notification.");

            await _unitOfWork.CompleteAsync();

            return Result<string>.Ok("Notification added successfully.");
        }
    }
}
