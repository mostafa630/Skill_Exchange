using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Notifications;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.Notifications.Queries.Handlers
{
    public class GetNotificationsByUserIdHandler
        : IRequestHandler<GetNotificationsByUserId, Result<List<NotificationDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetNotificationsByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<NotificationDto>>> Handle(GetNotificationsByUserId request, CancellationToken cancellationToken)
        {
            // Get the user with related notifications
            var userRepo = _unitOfWork.GetRepository<AppUser>();
            var notificationRepo = _unitOfWork.GetRepository<Notification>();

            var user = await userRepo.GetByIdAsync(request.userId);
            if (user == null)
                return Result<List<NotificationDto>>.Fail("User not found.");

            var allNotifications = await notificationRepo.GetAllAsync(); 
            var userNotifications = allNotifications
                .Where(n => n.Users != null && n.Users.Any(u => u.Id == request.userId))
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            var dtoList = _mapper.Map<List<NotificationDto>>(userNotifications);

            return Result<List<NotificationDto>>.Ok(dtoList);
        }
    }
}
