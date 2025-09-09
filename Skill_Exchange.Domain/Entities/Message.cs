using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Skill_Exchange.Domain.Entities
{
    public class Message
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Foreign keys (stored in Mongo)
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ConversationId { get; set; }

        // Navigation (not stored in Mongo)
        [BsonIgnore]
        public AppUser Sender { get; set; }

        [BsonIgnore]
        public AppUser Receiver { get; set; }

        [BsonIgnore]
        public Conversation Conversation { get; set; }
    }
}