using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string content);
    }
}
