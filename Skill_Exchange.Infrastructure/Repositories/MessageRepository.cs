using MongoDB.Driver;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
namespace Skill_Exchange.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageRepository(MongoDbContext context)
        {
            _messages = context.Messages ?? throw new ArgumentNullException(nameof(context.Messages));
        }

        // Add a new message
        public async Task AddMessageAsync(Message message)
        {
            message.Id = Guid.NewGuid();
            message.SentAt = DateTime.UtcNow;
            await _messages.InsertOneAsync(message);
        }

        // Get all messages in a conversation (latest first)
        public async Task<IEnumerable<Message>> GetConversationMessagesAsync(Guid conversationId)
        {
            if (conversationId == Guid.Empty) return Enumerable.Empty<Message>();

            var filter = Builders<Message>.Filter.Eq(m => m.ConversationId, conversationId);
            return await _messages.Find(filter)
                                  .SortByDescending(m => m.SentAt)
                                  .ToListAsync();
        }

        // Get all messages for a user (latest first)
        public async Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId)
        {
            if (userId == Guid.Empty) return Enumerable.Empty<Message>();

            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                Builders<Message>.Filter.Eq(m => m.ReceiverId, userId)
            );

            return await _messages.Find(filter)
                                  .SortByDescending(m => m.SentAt)
                                  .ToListAsync();
        }

        // Get messages paginated for a user
        public async Task<IEnumerable<Message>> GetUserMessagesPaginatedAsync(Guid userId, int page, int pageSize)
        {
            if (userId == Guid.Empty) return Enumerable.Empty<Message>();
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                Builders<Message>.Filter.Eq(m => m.ReceiverId, userId)
            );

            return await _messages.Find(filter)
                                  .SortByDescending(m => m.SentAt)
                                  .Skip((page - 1) * pageSize)
                                  .Limit(pageSize)
                                  .ToListAsync();
        }

        // Get a message by ID
        public async Task<Message?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;

            var filter = Builders<Message>.Filter.Eq(m => m.Id, id);
            return await _messages.Find(filter).FirstOrDefaultAsync();
        }

        // Update a message
        public async Task<bool> UpdateMessageAsync(Message message)
        {
            var filter = Builders<Message>.Filter.Eq(m => m.Id, message.Id);
            var result = await _messages.ReplaceOneAsync(filter, message);
            return result.ModifiedCount > 0;
        }

        // Delete a message
        public async Task<bool> DeleteMessageAsync(Guid id)
        {
            if (id == Guid.Empty) return false;

            var filter = Builders<Message>.Filter.Eq(m => m.Id, id);
            var result = await _messages.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        // Get undelivered messages for a user
        public async Task<IEnumerable<Message>> GetUndeliveredMessagesAsync(Guid userId)
        {
            if (userId == Guid.Empty) return Enumerable.Empty<Message>();

            var filter = Builders<Message>.Filter.And(
                Builders<Message>.Filter.Eq(m => m.ReceiverId, userId),
                Builders<Message>.Filter.Eq(m => m.DeliveredAt, null)
            );

            return await _messages.Find(filter)
                                  .SortBy(m => m.SentAt)
                                  .ToListAsync();
        }

        // Get WhatsApp-style conversation previews (latest message + participants), paginated
        public async Task<IEnumerable<(Guid ConversationId, Message LastMessage, List<Guid> Participants)>> GetUserConversationDataPaginatedAsync(Guid userId, int page, int pageSize)
        {
            if (userId == Guid.Empty) return Enumerable.Empty<(Guid, Message, List<Guid>)>();
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                Builders<Message>.Filter.Eq(m => m.ReceiverId, userId)
            );

            var aggregation = _messages.Aggregate()
                .Match(filter)
                .SortByDescending(m => m.SentAt)
                .Group(
                    m => m.ConversationId,
                    g => new
                    {
                        ConversationId = g.Key,
                        LastMessage = g.First(),
                        Participants = g.Select(x => x.SenderId)
                                        .Union(g.Select(x => x.ReceiverId))
                                        .Where(id => id != userId)
                                        .Distinct()
                                        .ToList()
                    }
                )
                .SortByDescending(c => c.LastMessage.SentAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize);

            var result = await aggregation.ToListAsync();
            return result.Select(a => (a.ConversationId, a.LastMessage, a.Participants));
        }
    }
}
