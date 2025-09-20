using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Auth
{
    public class ConfirmEmailRequestDto
    {
        public Guid UserId { get; set; }
        //public string Token { get; set; } = null!;
    }
}
