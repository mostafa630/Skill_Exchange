using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Infrastructure.Peresistence
{
    public class PendingVerificationRepository : GenericRepository<PendingVerification>,IPendingVerificationRepository
    {
        private readonly DbSet<PendingVerification> _dbSet;
        public PendingVerificationRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<PendingVerification>();
        }

        public Task<PendingVerification> GetByEmailAsync(string email)
        {
            return _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<PendingVerification> GetByVerificationCodeAsync(string verificationCode)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.VerificationCode == verificationCode);
        }
    }
}