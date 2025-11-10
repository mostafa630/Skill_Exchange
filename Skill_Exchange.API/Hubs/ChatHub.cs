using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration; 
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;
        private static readonly Dictionary<Guid, string> _connections = new();

        // 1. Private Fields to hold the securely loaded Twilio credentials
        private readonly string _twilioAccountSid;
        private readonly string _twilioAuthToken;

        // 2. Inject IConfiguration and read secrets
        public ChatHub(MessageService messageService, IConfiguration configuration)
        {
            _messageService = messageService;

            // Read secrets securely from configuration
            _twilioAccountSid = configuration["Twilio:AccountSid"]
                ?? throw new InvalidOperationException("Twilio:AccountSid not found. Check configuration.");

            _twilioAuthToken = configuration["Twilio:AuthToken"]
                ?? throw new InvalidOperationException("Twilio:AuthToken not found. Check configuration.");
        }

        // ------------------ WebRTC: Secure TURN Token Retrieval ------------------

        public async Task<object?> GetTwilioIceServers()
        {
            try
            {
                // Initialize Twilio client using the injected, secure fields
                TwilioClient.Init(_twilioAccountSid, _twilioAuthToken);

                // Request a new ephemeral (temporary) token from Twilio
                var tokenResource = await TokenResource.CreateAsync();

                // The returned object contains the fresh, non-expired ICE servers list.
                return new
                {
                    // The property name "ice_servers" must match what the JavaScript client expects.
                    ice_servers = tokenResource.IceServers,
                    ttl = tokenResource.Ttl,
                    username = tokenResource.Username,
                    credential = tokenResource.Password
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Twilio Token Error: {ex.Message}");
                // Return null so the frontend client knows the token retrieval failed.
                return null;
            }
        }

        // ------------------ Connection Handling ------------------

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

                await Clients.Caller.SendAsync("UserConnected", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!user.Equals(default(KeyValuePair<Guid, string>)))
            {
                _connections.Remove(user.Key);
                await Clients.All.SendAsync("UserDisconnected", user.Key);
            }

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

        // ------------------ Call Signaling (Audio/Video) ------------------

        // Initiates a call (audio or video)
        public async Task CallUser(Guid callerId, Guid targetUserId, string offer, string callType = "audio")
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("ReceiveCallOffer", new
                {
                    CallerId = callerId,
                    Offer = offer,
                    CallType = callType
                });
            }
            else
            {
                await Clients.Caller.SendAsync("UserUnavailable", targetUserId);
            }
        }

        // Answer an incoming call
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

        // Exchange ICE candidates between peers
        public async Task SendIceCandidate(Guid targetUserId, string candidate)
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("ReceiveIceCandidate", candidate);
            }
        }

        // End call for both users
        public async Task EndCall(Guid userId, Guid targetUserId)
        {
            if (_connections.TryGetValue(targetUserId, out var targetConnection))
            {
                await Clients.Client(targetConnection).SendAsync("CallEnded", userId);
            }

            await Clients.Caller.SendAsync("CallEnded", targetUserId);
        }

        // Notify caller if target rejects call
        public async Task RejectCall(Guid callerId, Guid receiverId)
        {
            if (_connections.TryGetValue(callerId, out var callerConnection))
            {
                await Clients.Client(callerConnection).SendAsync("CallRejected", receiverId);
            }
        }
    }
}