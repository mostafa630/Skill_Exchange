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
        public DateOnly DateOfBirth { get; set; }
        public string? Bio { get; set; }
        public DateTime LastActiveAt { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Navigation properties
        public ICollection<RatingAndFeedback> RatingsGiven { get; set; } = new List<RatingAndFeedback>();
        public ICollection<RatingAndFeedback> RatingsReceived { get; set; } = new List<RatingAndFeedback>();


        public ICollection<Request> RequestsSent { get; set; } = new List<Request>();
        public ICollection<Request> RequestsReceived { get; set; } = new List<Request>();

        public ICollection<Conversation> ConversationsAsA { get; set; } = new List<Conversation>();
        public ICollection<Conversation> ConversationsAsB { get; set; } = new List<Conversation>();

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<AppUser> Friends { get; set; } = new List<AppUser>();
        public ICollection<AppUser> FriendOf { get; set; } = new List<AppUser>();


    }
}