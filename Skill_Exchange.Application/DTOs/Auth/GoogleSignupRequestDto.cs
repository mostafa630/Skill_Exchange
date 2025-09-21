using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.Auth
{
    public class GoogleSignupRequestDto
    {
        public string Email { get; set; } = default!;
        public string IdToken { get; set; } = default!; // token from Google for verification
    }
}
