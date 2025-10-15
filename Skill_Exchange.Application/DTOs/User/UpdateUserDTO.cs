using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UpdateUserDTO

    {
        [JsonIgnore]  // hidden from clients
        public string Email { get; set; } = String.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? bio { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; } = null;
    }
}