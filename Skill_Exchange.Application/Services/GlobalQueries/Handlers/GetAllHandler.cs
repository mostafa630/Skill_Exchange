using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.GlobalQuery.Handlers
{
    public class GetAllHandler<T, TDTO> : IRequestHandler<GetAll<T, TDTO>, Result<IEnumerable<TDTO>>>
        where T : class
        where TDTO : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<TDTO>>> Handle(GetAll<T, TDTO> request, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _unitOfWork.GetRepository<T>().GetAllAsync(request.spec);
                var entitiesDtos = _mapper.Map<IEnumerable<TDTO>>(entities);
                return Result<IEnumerable<TDTO>>.Ok(entitiesDtos);
            }
            catch
            {
                return Result<IEnumerable<TDTO>>.Fail("Operation Failed");
            }
        }
    }

}