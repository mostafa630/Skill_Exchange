using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Services.GlobalCommands.Handlers
{
    public class DeleteHandler<T> : IRequestHandler<Delete<T>, Result<string>>
    where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(Delete<T> request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<T>();
            var entity = await repo.GetByIdAsync(request.Id);

            if (entity == null)
                return Result<string>.Fail("Entity not found.");

            await repo.DeleteAsync(entity);
            await _unitOfWork.CompleteAsync();

            return Result<string>.Ok("Entity deleted successfully.");
        }
    }

}
