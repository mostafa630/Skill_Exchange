using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class UserFriend
    {
        public Guid UserId { get; set; }
        public Guid FreindId { get; set; }

        // Navigation properties
        public AppUser User { get; set; }
        public AppUser Friend { get; set; }
    }
}