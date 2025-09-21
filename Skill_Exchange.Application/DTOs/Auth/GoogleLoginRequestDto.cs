using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Auth
{
    public class GoogleLoginRequestDto
    {
        public string IdToken { get; set; } = default!;  // Google returns this token
    }
}
