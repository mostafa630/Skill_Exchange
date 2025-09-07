using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Entities
{
    public class Friends
    {
        public Guid UserAId { get; set; }
        public Guid UserBId { get; set; }

        // Navigation properties
        public AppUser UserA { get; set; }
        public AppUser UserB { get; set; }
    }
}