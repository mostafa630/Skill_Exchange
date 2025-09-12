using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Infrastructure.Peresistence
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        private readonly DbSet<AppUser> _dbSet;
        public UserRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<AppUser>();
        }
        public Task<AppUser> GetByEmailAsync(string email)
        {
            return _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}