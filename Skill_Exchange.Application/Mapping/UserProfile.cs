using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Mapping
{
    public class UserResponseProfile : Profile
    {
        public UserResponseProfile()
        {
            CreateMap<AppUser, UserDTO>().ReverseMap();
        }
    }
}
