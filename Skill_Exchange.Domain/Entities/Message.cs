using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Skill_Exchange.Domain.Entities
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ReadAt { get; set; }

        // Foreign keys (stored in Mongo)
        [BsonRepresentation(BsonType.String)]
        public Guid SenderId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid ReceiverId { get; set; }
        [BsonRepresentation(BsonType.String)]
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