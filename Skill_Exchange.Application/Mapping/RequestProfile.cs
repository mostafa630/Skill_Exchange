using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Domain.Entities;
using AutoMapper;

namespace Skill_Exchange.Application.Mapping
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<CreateRequestDTO, Request>().ReverseMap();
            CreateMap<Request, RequestDTO>().ReverseMap();
        }
    }
}