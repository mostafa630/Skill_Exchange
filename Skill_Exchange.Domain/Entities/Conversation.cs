using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public Guid ParticipantAId { get; set; }
        public Guid ParticipantBId { get; set; }

        public AppUser ParticipantA { get; set; }
        public AppUser ParticipantB { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}