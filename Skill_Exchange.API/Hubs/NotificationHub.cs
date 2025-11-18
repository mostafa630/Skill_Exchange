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

                // Load existing notifications for the user
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

        // ------------------- Send Notification to One User -------------------
        public async Task SendToUser(Guid userId, NotificationDto dto)
        {
            // 1️ Fetch user
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return;

            // 2️ Add notification via MediatR
            var result = await _mediator.Send(new AddNotification(userId, dto.Title, dto.Message, dto.RefrenceId));
            if (!result.Success) return;

            // 3️ Deliver to online user
            if (_connections.TryGetValue(userId, out var connId))
            {
                await Clients.Client(connId).SendAsync("ReceiveNotification", dto);
            }
        }

        // ------------------- Send Notification to Multiple Users -------------------
        public async Task SendToUsers(List<Guid> userIds, NotificationDto dto)
        {
            // Fetch all target users at once
            var users = new List<AppUser>();
            foreach (var userId in userIds)
            {
                var u = await _unitOfWork.Users.GetByIdAsync(userId);
                if (u != null) users.Add(u);
            }

            // 1️ Create a single Notification and assign all target users
            if (!users.Any()) return;

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                RefrenceId = dto.RefrenceId,
                Users = users 
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            // 2️ Deliver to online users
            foreach (var user in users)
            {
                if (_connections.TryGetValue(user.Id, out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveNotification", dto);
                }
            }
        }

        // ------------------- Broadcast to All Users -------------------
        public async Task Broadcast(NotificationDto dto)
        {
            var allUsers = (await _unitOfWork.Users.GetAllAsync()).ToList();
            if (!allUsers.Any()) return;

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow,
                RefrenceId = dto.RefrenceId,
                Users = allUsers
            };

            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            // Deliver to online clients
            foreach (var user in allUsers)
            {
                if (_connections.TryGetValue(user.Id, out var connId))
                {
                    await Clients.Client(connId).SendAsync("ReceiveNotification", dto);
                }
            }
        }
    }
}
