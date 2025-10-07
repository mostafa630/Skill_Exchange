using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MediatR.Pipeline;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Request.Queries.Handlers
{
    public class GetRequestsSendedHandler : IRequestHandler<GetRequestsSended, Result<IEnumerable<RequestDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetRequestsSendedHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<RequestDTO>>> Handle(GetRequestsSended request, CancellationToken cancellationToken)
        {
            try
            {
                var requests = await _unitOfWork.Requests.GetRequestsSendedByAsync(request.SenderId);
                var requestsDTO = _mapper.Map<IEnumerable<RequestDTO>>(requests);
                return Result<IEnumerable<RequestDTO>>.Ok(requestsDTO);
            }
            catch
            {
                return Result<IEnumerable<RequestDTO>>.Fail("Operation failed");
            }
        }
    }
}