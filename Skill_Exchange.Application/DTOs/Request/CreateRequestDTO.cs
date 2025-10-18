
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Skill_Exchange.Domain.Enums;

namespace Skill_Exchange.Application.DTOs.Request
{
    public class CreateRequestDTO
    {
        [JsonIgnore]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        [JsonIgnore]
        [BindNever]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? RespondedAt { get; set; } = null;
        //Navigation properties
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}