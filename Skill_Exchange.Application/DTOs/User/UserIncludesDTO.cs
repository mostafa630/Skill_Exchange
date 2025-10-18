using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UserIncludesDTO
    {
        public bool RatingsGiven { get; set; }
        public bool RatingsReceived { get; set; }
        public bool RequestsSent { get; set; }
        public bool RequestsReceived { get; set; }
        public bool ConversationsAsA { get; set; }
        public bool ConversationsAsB { get; set; }
        public bool Skills { get; set; }
        public bool Notifications { get; set; }
        public bool FriendOf { get; set; }
        public bool Friends { get; set; }
    }
}