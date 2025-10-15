using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.Request
{
    public class ChangeRequestStatusDTO
    {
        public RequestStatus Status { get; set; }
    }
}