using MongoDB.Driver;
using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Skill_Exchange.Infrastructure.Peresistence
{
    public class MongoSeeder
    {
        private readonly MongoDbContext _context;

        public MongoSeeder(MongoDbContext context)
        {
            _context = context;
        }

        public void SeedMessages()
        {
            var messagesCollection = _context.Messages;

            if (messagesCollection.CountDocuments(_ => true) > 0)
                return; // Already seeded

            var messages = new List<Message>
            {
                new Message
                {
                    Id = Guid.NewGuid(),
                    Content = "Hello Farag!",
                    SentAt = DateTime.UtcNow.AddMinutes(-10),
                    DeliveredAt = DateTime.UtcNow.AddMinutes(-9),
                    ReadAt = DateTime.UtcNow.AddMinutes(-8),
                    SenderId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    ReceiverId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    ConversationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                },
                new Message
                {
                    Id = Guid.NewGuid(),
                    Content = "Hi! How are you?",
                    SentAt = DateTime.UtcNow.AddMinutes(-5),
                    DeliveredAt = DateTime.UtcNow.AddMinutes(-4),
                    ReadAt = DateTime.UtcNow.AddMinutes(-3),
                    SenderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    ReceiverId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    ConversationId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                }
            };

            messagesCollection.InsertMany(messages);
        }
    }
}
