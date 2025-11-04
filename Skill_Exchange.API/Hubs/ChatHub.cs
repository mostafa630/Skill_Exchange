using Microsoft.AspNetCore.SignalR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;
        private static readonly Dictionary<Guid, string> _connections = new();

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        // Called when a user connects
        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId) && userId != Guid.Empty)
            {
                _connections[userId] = Context.ConnectionId;

                // Send all undelivered messages to this user
                var undeliveredResult = await _messageService.GetUndeliveredMessagesAsync(userId);
                if (undeliveredResult.Success)
                {
                    foreach (var msg in undeliveredResult.Data)
                    {
                        // Send to connected user
                        await Clients.Caller.SendAsync("ReceiveMessage", msg);

                        // Mark as delivered now
                        msg.DeliveredAt = DateTime.UtcNow;
                        await _messageService.MarkMessageDeliveredAsync(msg.Id);
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        // Called when a user disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!user.Equals(default(KeyValuePair<Guid, string>)))
                _connections.Remove(user.Key);

            await base.OnDisconnectedAsync(exception);
        }

        // Send a message to a user
        public async Task<Result<MessageResponseDTO>> SendMessage(Guid receiverId, Guid senderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Result<MessageResponseDTO>.Fail("Message cannot be empty");

            if (receiverId == senderId)
                return Result<MessageResponseDTO>.Fail("You cannot send a message to yourself");

            try
            {
                // Save message in DB
                var sendResult = await _messageService.SendMessageAsync(senderId, receiverId, content);
                if (!sendResult.Success)
                    return Result<MessageResponseDTO>.Fail(sendResult.Error);

                var messageDto = sendResult.Data;

                // Real-time delivery if receiver is online
                if (_connections.TryGetValue(receiverId, out var connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", messageDto);

                    // Mark as delivered immediately
                    messageDto.DeliveredAt = DateTime.UtcNow;
                    await _messageService.MarkMessageDeliveredAsync(messageDto.Id);
                }

                // Notify sender that message was sent
                await Clients.Caller.SendAsync("MessageSent", messageDto);

                return Result<MessageResponseDTO>.Ok(messageDto);
            }
            catch (Exception ex)
            {
                return Result<MessageResponseDTO>.Fail(ex.Message);
            }
        }

        // Mark a message as read
        public async Task MarkAsRead(Guid messageId)
        {
            var updatedMessage = await _messageService.MarkMessageReadAsync(messageId);
            if (updatedMessage != null)
            {
                // Notify sender that message was read
                if (_connections.TryGetValue(updatedMessage.SenderId, out var senderConnection))
                {
                    await Clients.Client(senderConnection)
                        .SendAsync("MessageRead", updatedMessage);
                }
            }
        }
    }
}
