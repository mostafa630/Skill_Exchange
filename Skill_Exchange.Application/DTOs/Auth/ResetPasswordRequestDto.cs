using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public Guid Token { get; set; }
        public string NewPassword { get; set; }

    }
}