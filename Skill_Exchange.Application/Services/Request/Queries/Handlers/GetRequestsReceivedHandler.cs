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
    public class GetRequestsReceivedHandler : IRequestHandler<GetRequestsReceived, Result<IEnumerable<RequestDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetRequestsReceivedHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<RequestDTO>>> Handle(GetRequestsReceived request, CancellationToken cancellationToken)
        {
            try
            {
                var requests = await _unitOfWork.Requests.GetRequestsReceivedByAsync(request.RecieverId);

                var requestsDTO = _mapper.Map<IEnumerable<RequestDTO>>(requests);
                return Result<IEnumerable<RequestDTO>>.Ok(requestsDTO);
            }
            catch
            {
                System.Console.WriteLine("here 2");

                return Result<IEnumerable<RequestDTO>>.Fail("Operation failed");

            }
        }
    }
}