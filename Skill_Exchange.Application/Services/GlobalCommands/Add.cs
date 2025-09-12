using MediatR;
using Skill_Exchange.Application.DTOs.DTOInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.GlobalCommands
{
    public record Add<T, TCreateDTO, TCreateResponseDTO>(TCreateDTO CreateDTO) : IRequest<TCreateResponseDTO>
        where TCreateDTO : class, ICreate
        where T : class
        where TCreateResponseDTO : class;
}
