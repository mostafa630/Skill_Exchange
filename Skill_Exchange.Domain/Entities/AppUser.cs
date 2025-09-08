using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Skill_Exchange.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime LastActiveAt { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Navigation properties
        public ICollection<RatingAndFeedback> Rates { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Conversation> Conversations { get; set; }


        public ICollection<Skill> Skills { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<UserFriend> Freinds { get; set; }
    }
}