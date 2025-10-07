using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Request
{
    public class GetRequestBetweenDto
    {
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }

    }
}