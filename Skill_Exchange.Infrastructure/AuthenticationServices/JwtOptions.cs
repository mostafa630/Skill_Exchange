using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string LifeTime { get; set; }
        public string SigningKey { get; set; }
    }
}