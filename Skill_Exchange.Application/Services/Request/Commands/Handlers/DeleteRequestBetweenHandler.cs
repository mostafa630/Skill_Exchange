using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.Request.Commands.Handlers
{
    public class DeleteRequestBetweenHandler : IRequestHandler<DeleteRequestBetween, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteRequestBetweenHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(DeleteRequestBetween request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _unitOfWork.Requests.DeleteRequestBetween(request.delteRequestBetweenDTO.User1Id, request.delteRequestBetweenDTO.User2Id);
                return res ? Result<bool>.Ok(res) : Result<bool>.Fail("Deletion Operation Failed or No Request between Them");
            }
            catch
            {
                return Result<bool>.Fail("Deletion Operation Failed or No Request between Them");
            }

        }
    }
}