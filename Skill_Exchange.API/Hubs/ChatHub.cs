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

        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId) && userId != Guid.Empty)
            {
                _connections[userId] = Context.ConnectionId;

                // Send undelivered messages
                var undeliveredResult = await _messageService.GetUndeliveredMessagesAsync(userId);
                if (undeliveredResult.Success)
                {
                    foreach (var msg in undeliveredResult.Data)
                    {
                        await Clients.Caller.SendAsync("ReceiveMessage", msg);
                        msg.DeliveredAt = DateTime.UtcNow;
                        await _messageService.MarkMessageDeliveredAsync(msg.Id);
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!user.Equals(default(KeyValuePair<Guid, string>)))
                _connections.Remove(user.Key);

            await base.OnDisconnectedAsync(exception);
        }

        // ------------------ Chat ------------------

        public async Task<Result<MessageResponseDTO>> SendMessage(Guid receiverId, Guid senderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Result<MessageResponseDTO>.Fail("Message cannot be empty");

            if (receiverId == senderId)
                return Result<MessageResponseDTO>.Fail("You cannot send a message to yourself");

            try
            {
                var sendResult = await _messageService.SendMessageAsync(senderId, receiverId, content);
                if (!sendResult.Success)
                    return Result<MessageResponseDTO>.Fail(sendResult.Error);

                var messageDto = sendResult.Data;

                // Deliver if receiver online
                if (_connections.TryGetValue(receiverId, out var connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", messageDto);
                    messageDto.DeliveredAt = DateTime.UtcNow;
                    await _messageService.MarkMessageDeliveredAsync(messageDto.Id);
                }

                await Clients.Caller.SendAsync("MessageSent", messageDto);
                return Result<MessageResponseDTO>.Ok(messageDto);
            }
            catch (Exception ex)
            {
                return Result<MessageResponseDTO>.Fail(ex.Message);
            }
        }

        public async Task MarkAsRead(Guid messageId)
        {
            var updatedMessage = await _messageService.MarkMessageReadAsync(messageId);
            if (updatedMessage != null)
            {
                if (_connections.TryGetValue(updatedMessage.SenderId, out var senderConnection))
                {
                    await Clients.Client(senderConnection).SendAsync("MessageRead", updatedMessage);
                }
            }
        }

        // ------------------ Call Signaling ------------------

        public async Task CallUser(Guid callerId, Guid targetUserId, string offer)
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("ReceiveCallOffer", new
                {
                    CallerId = callerId,
                    Offer = offer
                });
            }
            else
            {
                await Clients.Caller.SendAsync("UserUnavailable", targetUserId);
            }
        }

        public async Task AnswerCall(Guid callerId, Guid receiverId, string answer)
        {
            if (_connections.TryGetValue(callerId, out var callerConnection))
            {
                await Clients.Client(callerConnection).SendAsync("ReceiveCallAnswer", new
                {
                    ReceiverId = receiverId,
                    Answer = answer
                });
            }
        }

        public async Task SendIceCandidate(Guid targetUserId, string candidate)
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("ReceiveIceCandidate", candidate);
            }
        }

        public async Task EndCall(Guid userId, Guid targetUserId)
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("CallEnded", userId);
            }

            await Clients.Caller.SendAsync("CallEnded", targetUserId);
        }
    }
}
