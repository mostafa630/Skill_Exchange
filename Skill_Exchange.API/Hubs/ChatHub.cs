using Azure;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services.Conversation.Queries;
using Skill_Exchange.Application.Services.GlobalCommands;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private static readonly Dictionary<Guid, string> _connections = new(); 

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId) && userId != Guid.Empty)
            {
                _connections[userId] = Context.ConnectionId;
                Console.WriteLine($"User {userId} connected with {Context.ConnectionId}");
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

        public async Task SendMessage(Guid receiverId, Guid senderId, string message)
        {
            if (_connections.TryGetValue(receiverId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
            }

            // ✅ Save message in DB
            var query = new GetConversation(senderId, receiverId);
            var result = await _mediator.Send(query);
            var conversationId = result.Data;
            var createDto = new CreateMessageDTO
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ConversationId = conversationId,
                Content = message
            };

            var dbResult= await _mediator.Send(
                new Add<Message, CreateMessageDTO, MessageResponseDTO>(createDto)
            );

            if (!dbResult.Success)
            {
                await Clients.Caller.SendAsync("Error", result.Error);
            }
        }
    }
}
