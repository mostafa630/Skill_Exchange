using Microsoft.AspNetCore.SignalR;
using MediatR;
using Skill_Exchange.Application.DTOs.Notifications;
using Skill_Exchange.Application.Services.Notifications.Commands;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.API.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private static readonly Dictionary<Guid, string> _connections = new();

        public NotificationHub(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId)
                && userId != Guid.Empty)
            {
                _connections[userId] = Context.ConnectionId;

                // Load existing notifications for the user from DB
                var allNotifications = await _unitOfWork.Notifications.GetAllAsync();
                var userNotifications = allNotifications
                    .Where(n => n.Users != null && n.Users.Any(u => u.Id == userId))
                    .OrderByDescending(n => n.CreatedAt)
                    .ToList();

                foreach (var notif in userNotifications)
                {
                    await Clients.Caller.SendAsync("ReceiveNotification", new NotificationDto
                    {
                        Id = notif.Id,
                        Title = notif.Title,
                        Message = notif.Message,
                        CreatedAt = notif.CreatedAt,
                        RefrenceId = notif.RefrenceId
                    });
                }

                await Clients.Caller.SendAsync("UserConnected", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(c => c.Value == Context.ConnectionId);
            if (!user.Equals(default(KeyValuePair<Guid, string>)))
            {
                _connections.Remove(user.Key);
                await Clients.All.SendAsync("UserDisconnected", user.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // ------------------- Send Existing Notification to One User -------------------
        public async Task SendExistingNotification(Guid notificationId, Guid userId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null) return;

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return;

            // Link user via the many-to-many automatically created by EF Core
            if (!notification.Users.Any(u => u.Id == user.Id))
            {
                notification.Users.Add(user);
                await _unitOfWork.CompleteAsync(); // persist link in join table
            }

            // Deliver in real-time if online
            if (_connections.TryGetValue(user.Id, out var connId))
            {
                await Clients.Client(connId).SendAsync("ReceiveNotification", new NotificationDto
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt,
                    RefrenceId = notification.RefrenceId
                });
            }
        }

        // ------------------- Send Existing Notification to Multiple Users -------------------
        public async Task SendExistingNotificationToUsers(Guid notificationId, List<Guid> userIds)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null) return;

            var users = new List<AppUser>();
            foreach (var userId in userIds)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user != null) users.Add(user);
            }

            if (!users.Any()) return;

            bool updated = false;
            foreach (var user in users)
            {
                if (!notification.Users.Any(u => u.Id == user.Id))
                {
                    notification.Users.Add(user); // EF Core handles join table
                    updated = true;
                }
            }

            if (updated)
                await _unitOfWork.CompleteAsync();

            // Deliver to online users
            foreach (var user in users)
            {
                if (_connections.TryGetValue(user.Id, out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveNotification", new NotificationDto
                    {
                        Id = notification.Id,
                        Title = notification.Title,
                        Message = notification.Message,
                        CreatedAt = notification.CreatedAt,
                        RefrenceId = notification.RefrenceId
                    });
                }
            }
        }

        // ------------------- Send New Notification to One User -------------------
        public async Task SendNewNotification(Guid userId, NotificationDto dto)
        {
            var result = await _mediator.Send(new AddNotification(userId, dto.Title, dto.Message, dto.RefrenceId));
            if (!result.Success) return;

            // Deliver real-time if online
            if (_connections.TryGetValue(userId, out var connId))
            {
                await Clients.Client(connId).SendAsync("ReceiveNotification", dto);
            }
        }

        // ------------------- Send Notification Based on Condition -------------------
        public async Task SendNotificationToUsersWhere(Func<AppUser, bool> condition, NotificationDto dto)
        {
            var allUsers = await _unitOfWork.Users.GetAllAsync();
            var users = allUsers.Where(condition).ToList();

            if (!users.Any()) return;

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                RefrenceId = dto.RefrenceId,
                Users = users // EF Core will create UserNotification links
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            foreach (var user in users)
            {
                if (_connections.TryGetValue(user.Id, out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveNotification", dto);
                }
            }
        }
    }
}
