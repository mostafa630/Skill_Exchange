using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Infrastructure.AuthenticationServices
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}