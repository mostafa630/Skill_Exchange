using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        public readonly string Email;
        public string? FirstName;
        public string? LastName;

        public DateOnly? DateOfBirth { get; set; }
    }
}