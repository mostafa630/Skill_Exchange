using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        public Task<AppUser> GetByEmailAsync(string email);
    }
}