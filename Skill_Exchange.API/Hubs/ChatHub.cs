using Microsoft.AspNetCore.SignalR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services;
using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly Dictionary<Guid, string> _connections = new();

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            if (Guid.TryParse(Context.GetHttpContext()?.Request.Query["userId"], out Guid userId) &&
                userId != Guid.Empty)
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

        public async Task<Result<Message>> SendMessage(Guid receiverId, Guid senderId, Guid conversationId, string content)
        {
            //  1: Immediately send message to receiver (if online)
            if (_connections.TryGetValue(receiverId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, content);
            }

            //  2: Save message in DB using MessageService + Result pattern
            var saveResult = await _messageService.SendMessageAsync(senderId, receiverId, content, conversationId);

            //  3: Send confirmation or error back to sender
            if (!saveResult.Success)
            {
                await Clients.Caller.SendAsync("Error", saveResult.Error);
                return Result<Message>.Fail(saveResult.Error);
            }

            await Clients.Caller.SendAsync("MessageSent", saveResult.Data);
            return Result<Message>.Ok(saveResult.Data);
        }
    }
}
