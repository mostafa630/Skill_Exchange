using Microsoft.EntityFrameworkCore;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure.Peresistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Infrastructure.Repositories
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        private readonly DbSet<Conversation> _dbSet;
        public ConversationRepository(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<Conversation>();
        }
        public async Task<Guid> GetConversationIdBetween(Guid user1Id, Guid user2Id)
        {
            var conversation = _dbSet.FirstOrDefault(c =>
                (c.ParticipantAId == user1Id && c.ParticipantBId == user2Id) ||
                (c.ParticipantBId == user1Id && c.ParticipantAId == user2Id));

            return conversation?.Id ?? Guid.Empty; 
        }

    }
}
