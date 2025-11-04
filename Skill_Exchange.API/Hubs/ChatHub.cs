using MediatR;
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
                        await Clients.Caller.SendAsync("ReceiveMessage", msg);

                        // Mark as delivered
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
                // Save message using MessageService
                var sendResult = await _messageService.SendMessageAsync(senderId, receiverId, content);
                if (!sendResult.Success)
                    return Result<MessageResponseDTO>.Fail(sendResult.Error);

                var messageDto = sendResult.Data;

                // Real-time send if receiver is online
                if (_connections.TryGetValue(receiverId, out var connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", messageDto);

                    // Mark as delivered since user is online
                    await _messageService.MarkMessageDeliveredAsync(messageDto.Id);

                    // Update DTO locally
                    messageDto.DeliveredAt = DateTime.UtcNow;
                }

                // Optional: confirmation back to sender
                await Clients.Caller.SendAsync("MessageSent", messageDto);

                return Result<MessageResponseDTO>.Ok(messageDto);
            }
            catch (Exception ex)
            {
                return Result<MessageResponseDTO>.Fail(ex.Message);
            }
        }

        // Called when a user reads a message
        public async Task MarkAsRead(Guid messageId)
        {
            await _messageService.MarkMessageReadAsync(messageId);
        }
    }
}
