using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId);
        Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId);
        Task<Message?> GetByIdAsync(Guid id);
        Task<bool> UpdateMessageAsync(Message message);
        Task<bool> DeleteMessageAsync(Guid id);
        Task<IEnumerable<Message>> GetUndeliveredMessagesAsync(Guid userId);
        Task<IEnumerable<Message>> GetUserMessagesPaginatedAsync(Guid userId, int page, int pageSize);
        Task<IEnumerable<(Guid ConversationId, Message LastMessage, List<Guid> Participants)>> GetUserConversationDataPaginatedAsync(Guid userId, int page, int pageSize);

    }
}
