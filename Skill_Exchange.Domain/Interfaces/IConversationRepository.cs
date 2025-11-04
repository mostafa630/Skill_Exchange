using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Domain.Interfaces
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        public Task<Guid> GetConversationIdBetween(Guid user1Id, Guid user2Id);
    }
}
