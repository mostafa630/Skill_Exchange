using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
