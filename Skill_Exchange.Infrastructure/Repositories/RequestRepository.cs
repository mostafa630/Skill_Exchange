using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Infrastructure.Peresistence;

namespace Skill_Exchange.Infrastructure.Repositories
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        private readonly DbSet<Request> _dbSet;
        public RequestRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<Request>();
        }

        public async Task<Request> GetRequestBetweenAsync(Guid user1Id, Guid user2Id)
        {
            return await _dbSet.FirstOrDefaultAsync(r =>
                (r.SenderId == user1Id && r.RecieverId == user2Id)
                || (r.SenderId == user2Id && r.RecieverId == user1Id)
            ); 
        }

        public async Task<IEnumerable<Request>> GetRequestsReceivedByAsync(Guid RecieverId)
        {
            return await _dbSet.Where(r => r.RecieverId == RecieverId).ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetRequestsSendedByAsync(Guid SenderId)
        {
            return await _dbSet.Where(r => r.SenderId == SenderId).ToListAsync();
        }

    }
}