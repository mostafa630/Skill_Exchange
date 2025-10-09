using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IUserSkillRepository : IGenericRepository<UserSkills>
    {
        Task<IEnumerable<UserSkills>> GetUserSkillsAsync(Guid userId, string? exchangePurpose = null);
        Task<UserSkills?> GetUserSkillAsync(Guid userId, Guid skillId);
        Task<bool> UserHasSkillAsync(Guid userId, Guid skillId);
    }
}