using MediatR;
using Microsoft.AspNetCore.SignalR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.Services;
using Skill_Exchange.Application.Services.Conversation.Queries;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly IMediator _mediator;
        private static readonly Dictionary<Guid, string> _connections = new();

        public ChatHub(MessageService messageService, IMediator mediator)
        {
            _messageService = messageService;
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId) && userId != Guid.Empty)
            {
                _connections[userId] = Context.ConnectionId;
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

        public async Task<Result<Message>> SendMessage(Guid receiverId, Guid senderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Result<Message>.Fail("Message cannot be empty");
            }

            var sentAt = DateTime.UtcNow;

            // Real-time send
            if (_connections.TryGetValue(receiverId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, content, sentAt);
            }

            await Clients.Caller.SendAsync("MessageSent", content, sentAt);

            try
            {
                // Get or create conversation
                var query = new GetConversation(senderId, receiverId);
                var conversationResult = await _mediator.Send(query);

                if (!conversationResult.Success)
                    return Result<Message>.Fail("Failed to get conversation");

                var conversationId = conversationResult.Data;

                // Save message in DB
                var sendResult = await _messageService.SendMessageAsync(senderId, receiverId, content, conversationId);

                if (!sendResult.Success)
                    return Result<Message>.Fail(sendResult.Error);

                return Result<Message>.Ok(sendResult.Data);
            }
            catch (Exception ex)
            {
                return Result<Message>.Fail(ex.Message);
            }
        }
    }
}
