
using AutoMapper;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.Request;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Request.Queries.Handlers
{
    public class GetRequestBetweenHandler : IRequestHandler<GetRequestBetween, Result<RequestDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetRequestBetweenHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<RequestDTO>> Handle(GetRequestBetween request, CancellationToken cancellationToken)
        {
            try
            {
                var res_request = await _unitOfWork.Requests.GetRequestBetweenAsync(request.getRequestBetweenDto.User1Id, request.getRequestBetweenDto.User2Id);
                if (res_request is null)
                {
                    return Result<RequestDTO>.Fail("no Request between them");
                }

                var requestDto = _mapper.Map<RequestDTO>(res_request);
                return Result<RequestDTO>.Ok(requestDto);

            }
            catch
            {
                return Result<RequestDTO>.Fail("Operation Failed");
            }
        }
    }
}