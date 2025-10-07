using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.Request
{
    public class RequestDTO
    {
        public Guid Id { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }

        //Navigation properties
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}