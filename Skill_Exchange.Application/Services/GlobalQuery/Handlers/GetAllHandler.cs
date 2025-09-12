using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.GlobalQuery.Handlers
{
    public class GetAllHandler<T, TDTO> : IRequestHandler<GetAll<T, TDTO>, IEnumerable<TDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TDTO>> Handle(GlobalQuery.GetAll<T> request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.GetRepository<T>().GetAllAsync();
            var entitiesDtos = _mapper.Map<IEnumerable<TDTO>>(entities);
            return entitiesDtos;
        }
    }
}