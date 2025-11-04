using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Result<Message>> SendMessageAsync(Guid senderId, Guid receiverId, string content, Guid conversationId)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Result<Message>.Fail("Message content cannot be empty.");

            if (senderId == Guid.Empty || receiverId == Guid.Empty)
                return Result<Message>.Fail("Invalid sender or receiver ID.");

            if (conversationId == Guid.Empty)
                return Result<Message>.Fail("Invalid conversation ID.");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content.Trim(),
                ConversationId = conversationId,
                SentAt = DateTime.UtcNow,
                ReadAt = null
            };

            await _messageRepository.AddMessageAsync(message);
            return Result<Message>.Ok(message);
        }

        public async Task<Result<IEnumerable<Message>>> GetConversationMessagesAsync(Guid conversationId)
        {
            if (conversationId == Guid.Empty)
                return Result<IEnumerable<Message>>.Fail("Invalid conversation ID.");

            var messages = await _messageRepository.GetConversationMessagesAsync(conversationId);

            if (!messages.Any())
                return Result<IEnumerable<Message>>.Fail("No messages found for this conversation.");

            return Result<IEnumerable<Message>>.Ok(messages);
        }

        public async Task<Result<IEnumerable<Message>>> GetUserMessagesAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return Result<IEnumerable<Message>>.Fail("Invalid user ID.");

            var messages = await _messageRepository.GetUserMessagesAsync(userId);

            if (!messages.Any())
                return Result<IEnumerable<Message>>.Fail("No messages found for this user.");

            return Result<IEnumerable<Message>>.Ok(messages);
        }

        public async Task<Result<bool>> UpdateMessageAsync(Guid messageId, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                return Result<bool>.Fail("Message content cannot be empty.");

            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null)
                return Result<bool>.Fail("Message not found.");

            message.Content = newContent.Trim();
            message.SentAt = DateTime.UtcNow; // optional: track edit time

            var updated = await _messageRepository.UpdateMessageAsync(message);

            return updated
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to update message.");
        }

        public async Task<Result<bool>> DeleteMessageAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<bool>.Fail("Invalid message ID.");

            var deleted = await _messageRepository.DeleteMessageAsync(id);

            return deleted
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to delete message.");
        }
    }
}
