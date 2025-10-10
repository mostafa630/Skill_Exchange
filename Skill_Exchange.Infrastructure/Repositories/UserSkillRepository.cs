
using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Enums;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure.Peresistence;

namespace Skill_Exchange.Infrastructure.Repositories
{
    public class UserSkillRepository : GenericRepository<UserSkills>, IUserSkillRepository
    {
        private readonly DbSet<UserSkills> _dbSet;
        public UserSkillRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<UserSkills>();
        }
        public async Task<UserSkills?> GetUserSkillAsync(Guid userId, Guid skillId)
        {
            return await _context.UserSkills
            .Include(us => us.Skill)
            .ThenInclude(s => s.SkillCategory)
            .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);
        }

        public async Task<IEnumerable<UserSkills>> GetUserSkillsAsync(Guid userId, string? exchangePurpose = null)
        {
            var query = _context.UserSkills
            .Include(us => us.Skill)
            .ThenInclude(s => s.SkillCategory)
            .Where(us => us.UserId == userId);

            if (!String.IsNullOrEmpty(exchangePurpose))
            {
                query = query.Where(us => us.Purpose.ToString() == exchangePurpose);
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<AppUser>> GetUsersBySkillAsync(Guid skillId, string? exchangePurpose = null)
        {
            var query = _context.UserSkills
            .Include(us => us.User)
            .Where(us => us.SkillId == skillId);

            if (!String.IsNullOrEmpty(exchangePurpose))
            {
                query = query.Where(us => us.Purpose.ToString() == exchangePurpose);
            }

            return await query
            .Select(us => us.User)
            .Distinct()
            .ToListAsync();
        }

        public async Task<bool> UserHasSkillAsync(Guid userId, Guid skillId)
        {
            return await _context.UserSkills
            .AnyAsync(us => us.UserId == userId && us.SkillId == skillId);
        }
    }
}