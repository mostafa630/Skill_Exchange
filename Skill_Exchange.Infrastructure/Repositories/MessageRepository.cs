using MongoDB.Driver;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure.Peresistence;

namespace Skill_Exchange.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageRepository(MongoDbContext context)
        {
            _messages = context.Messages;
        }

        public async Task AddMessageAsync(Message message)
        {
            message.Id = Guid.NewGuid();
            message.SentAt = DateTime.UtcNow;
            await _messages.InsertOneAsync(message);
        }

        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.ConversationId, conversationId);
            return await _messages.Find(filter).SortBy(m => m.SentAt).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId)
        {
            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                Builders<Message>.Filter.Eq(m => m.ReceiverId, userId)
            );
            return await _messages.Find(filter).SortByDescending(m => m.SentAt).ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(Guid id)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.Id, id);
            return await _messages.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMessageAsync(Message message)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.Id, message.Id);
            var result = await _messages.ReplaceOneAsync(filter, message);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteMessageAsync(Guid id)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.Id, id);
            var result = await _messages.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
