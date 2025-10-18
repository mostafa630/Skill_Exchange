using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UserFilterDTO
    {
        [JsonIgnore]
        [BindNever]
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // Filter by substring in bio (optional)
        public string? BioContains { get; set; }

        // Activity-based filters
        public DateTime? ActiveAfter { get; set; }
        public DateTime? ActiveBefore { get; set; }

        public bool? HasProfileImage { get; set; }
        public bool? GetFriends { get; set; }
    }
}