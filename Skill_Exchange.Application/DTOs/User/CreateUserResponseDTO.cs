using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.DTOs.User
{
    public class CreateUserResponseDTO
    {
        /* public string AccessToken { get; set; } = default!;
         public string RefreshToken { get; set; } = default!;
         public DateTime Expiration { get; set; }*/
        public UserDTO userDTO { get; set; }

    }
}
