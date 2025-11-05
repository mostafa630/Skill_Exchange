using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Conversation;
using Skill_Exchange.Application.DTOs.Message;
using Skill_Exchange.Application.Services.Conversation.Queries;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMediator mediator, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mediator = mediator;
            _mapper = mapper;
        }

        // Send a message between users
        public async Task<Result<MessageResponseDTO>> SendMessageAsync(Guid senderId, Guid receiverId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Result<MessageResponseDTO>.Fail("Message content cannot be empty.");

            if (senderId == Guid.Empty || receiverId == Guid.Empty)
                return Result<MessageResponseDTO>.Fail("Invalid sender or receiver ID.");

            var conversationResult = await _mediator.Send(new GetConversation(senderId, receiverId));
            if (!conversationResult.Success || conversationResult.Data == Guid.Empty)
                return Result<MessageResponseDTO>.Fail("You must be friends to send messages.");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content.Trim(),
                ConversationId = conversationResult.Data,
                SentAt = DateTime.UtcNow,
                DeliveredAt = null,
                ReadAt = null
            };

            await _messageRepository.AddMessageAsync(message);

            var dto = _mapper.Map<MessageResponseDTO>(message);
            return Result<MessageResponseDTO>.Ok(dto);
        }

        // Get all messages in a conversation
        public async Task<Result<IEnumerable<MessageResponseDTO>>> GetConversationMessagesAsync(Guid conversationId)
        {
            if (conversationId == Guid.Empty)
                return Result<IEnumerable<MessageResponseDTO>>.Fail("Invalid conversation ID.");

            var messages = await _messageRepository.GetConversationMessagesAsync(conversationId);
            if (!messages.Any())
                return Result<IEnumerable<MessageResponseDTO>>.Fail("No messages found for this conversation.");

            var dtoList = _mapper.Map<IEnumerable<MessageResponseDTO>>(messages);
            return Result<IEnumerable<MessageResponseDTO>>.Ok(dtoList);
        }

        // Get paginated messages for a user
        public async Task<Result<IEnumerable<MessageResponseDTO>>> GetUserMessagesPaginatedAsync(Guid userId, PaginationDto pagination)
        {
            if (userId == Guid.Empty)
                return Result<IEnumerable<MessageResponseDTO>>.Fail("Invalid user ID.");

            if (pagination == null)
                pagination = new PaginationDto();

            IEnumerable<Message> messages;
            if (pagination.ApplyPagination)
            {
                messages = await _messageRepository.GetUserMessagesPaginatedAsync(
                    userId,
                    pagination.Skip,
                    pagination.Take
                );
            }
            else
            {
                messages = await _messageRepository.GetUserMessagesAsync(userId);
            }

            if (!messages.Any())
                return Result<IEnumerable<MessageResponseDTO>>.Fail("No messages found for this user.");

            var dtoList = _mapper.Map<IEnumerable<MessageResponseDTO>>(messages);
            return Result<IEnumerable<MessageResponseDTO>>.Ok(dtoList);
        }

        // Get undelivered messages
        public async Task<Result<IEnumerable<MessageResponseDTO>>> GetUndeliveredMessagesAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return Result<IEnumerable<MessageResponseDTO>>.Fail("Invalid user ID");

            var messages = await _messageRepository.GetUndeliveredMessagesAsync(userId);
            var dtoList = _mapper.Map<IEnumerable<MessageResponseDTO>>(messages);
            return Result<IEnumerable<MessageResponseDTO>>.Ok(dtoList);
        }

        // Mark a message delivered
        public async Task MarkMessageDeliveredAsync(Guid messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message != null && message.DeliveredAt == null)
            {
                message.DeliveredAt = DateTime.UtcNow;
                await _messageRepository.UpdateMessageAsync(message);
            }
        }

        // Mark a message read
        public async Task<MessageResponseDTO?> MarkMessageReadAsync(Guid messageId)
        {
            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message != null && message.ReadAt == null)
            {
                message.ReadAt = DateTime.UtcNow;
                var updated = await _messageRepository.UpdateMessageAsync(message);
                if (updated)
                    return _mapper.Map<MessageResponseDTO>(message);
            }

            return message != null ? _mapper.Map<MessageResponseDTO>(message) : null;
        }

        // Update a message
        public async Task<Result<bool>> UpdateMessageAsync(Guid messageId, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                return Result<bool>.Fail("Message content cannot be empty.");

            var message = await _messageRepository.GetByIdAsync(messageId);
            if (message == null)
                return Result<bool>.Fail("Message not found.");

            message.Content = newContent.Trim();
            message.SentAt = DateTime.UtcNow; // track edit time

            var updated = await _messageRepository.UpdateMessageAsync(message);
            return updated
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to update message.");
        }

        // Delete a message
        public async Task<Result<bool>> DeleteMessageAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<bool>.Fail("Invalid message ID.");

            var deleted = await _messageRepository.DeleteMessageAsync(id);
            return deleted
                ? Result<bool>.Ok(true)
                : Result<bool>.Fail("Failed to delete message.");
        }

        // Get conversation previews (WhatsApp style) with pagination
        public async Task<Result<IEnumerable<ConversationPreviewDTO>>> GetConversationPreviewsAsync(Guid userId, int page, int pageSize)
        {
            if (userId == Guid.Empty)
                return Result<IEnumerable<ConversationPreviewDTO>>.Fail("Invalid user ID");

            var conversations = await _messageRepository.GetUserConversationDataPaginatedAsync(userId, page, pageSize);
            if (!conversations.Any())
                return Result<IEnumerable<ConversationPreviewDTO>>.Fail("No conversations found.");

            var dtoList = conversations.Select(c => new ConversationPreviewDTO
            {
                ConversationId = c.ConversationId,
                LastMessage = c.LastMessage.Content,          
                LastMessageSentAt = c.LastMessage.SentAt,    
                Participants = c.Participants
            });

            return Result<IEnumerable<ConversationPreviewDTO>>.Ok(dtoList);
        }
    }

    
}
