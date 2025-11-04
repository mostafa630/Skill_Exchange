using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> SendMessageAsync(Guid senderId, Guid receiverId, string content, Guid conversationId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be empty.");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content.Trim(),
                ConversationId = conversationId,
                ReadAt = null
            };

            await _messageRepository.AddMessageAsync(message);
            return message;
        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId)
        {
            if (conversationId == Guid.Empty)
                throw new ArgumentException("Invalid conversation ID.");

            return await _messageRepository.GetConversationMessagesAsync(conversationId);
        }

        public async Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.");

            return await _messageRepository.GetUserMessagesAsync(userId);
        }

        public async Task<bool> UpdateMessageAsync(Guid messageId, string newContent)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null) return false;

            message.Content = newContent;
            message.SentAt = DateTime.UtcNow; // optional: track edit time

            return await _messageRepository.UpdateMessageAsync(message);
        }

        public async Task<bool> DeleteMessageAsync(Guid id)
        {
            return await _messageRepository.DeleteMessageAsync(id);
        }
    }
}
