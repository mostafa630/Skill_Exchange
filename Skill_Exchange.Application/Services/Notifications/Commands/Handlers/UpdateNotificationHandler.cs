using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Notifications.Commands.Handlers
{
    public class UpdateNotificationHandler : IRequestHandler<UpdateNotification, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateNotificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateNotification request, CancellationToken cancellationToken)
        {
            var dto = request.updateNotificationDto;
            var repo = _unitOfWork.GetRepository<Notification>();

            //  Get the notification
            var existing = await repo.GetByIdAsync(dto.Id);
            if (existing == null)
                return Result<string>.Fail("Notification not found.");

            //  Update fields
            existing.Title = dto.Title;
            existing.Message = dto.Message;
            existing.RefrenceId = dto.RefrenceId;
            existing.CreatedAt = dto.CreatedAt;

            //  Save changes
            await repo.UpdateAsync(existing);
            var updated = await _unitOfWork.CompleteAsync();

            if (updated == 0)
                return Result<string>.Fail("No changes were made.");

            return Result<string>.Ok("Notification updated successfully.");
        }
    }
}
