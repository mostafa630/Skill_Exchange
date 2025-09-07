using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Domain.Entities
{
    public class Request
    {
        public Guid Id { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RespondedAt { get; set; }
    }
}