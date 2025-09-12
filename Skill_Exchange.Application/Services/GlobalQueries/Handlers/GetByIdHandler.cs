using AutoMapper;
using MediatR;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.GlobalQuery.Handlers
{
    public class GetByIdHandler<T, TDTO> : IRequestHandler<GetById<T, TDTO>, TDTO>
        where T : class
        where TDTO : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TDTO> Handle(GetById<T, TDTO> request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.GetRepository<T>().GetByIdAsync(request.Id);
            var entityDto = _mapper.Map<TDTO>(entity);
            return entityDto;
        }
    }
}
