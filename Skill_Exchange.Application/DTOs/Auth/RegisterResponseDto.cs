using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public string Token { get; set; } = default!;
        public string message { get; set; }
        public DateTime Expiration { get; set; }
    }
}
