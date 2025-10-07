
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.Request
{
    public class CreateRequestDTO
    {
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }

        //Navigation properties
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}