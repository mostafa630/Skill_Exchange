using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; } = null;
    }
}