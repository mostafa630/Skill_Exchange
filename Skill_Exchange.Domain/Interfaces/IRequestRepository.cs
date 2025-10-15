using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        Task<Request> GetRequestBetweenAsync(Guid user1Id, Guid user2Id);
        Task<IEnumerable<Request>> GetRequestsSendedByAsync(Guid SenderId);
        Task<IEnumerable<Request>> GetRequestsReceivedByAsync(Guid RecieverId);
        Task<bool> DeleteRequestBetween(Guid user1Id, Guid user2Id);
    }
}