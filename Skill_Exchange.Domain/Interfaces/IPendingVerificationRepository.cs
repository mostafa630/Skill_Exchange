using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IPendingVerificationRepository:IGenericRepository<PendingVerification>
    {
        Task<PendingVerification> GetByVerificationCodeAsync(string verificationCode);
        Task<PendingVerification> GetByEmailAsync(string email);

    }
}
