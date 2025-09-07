using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public DateTime ReadAt { get; set; }

        // Navigation properties
        public Guid SenderId { get; set; }
        public AppUser Sender { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }

    }
}