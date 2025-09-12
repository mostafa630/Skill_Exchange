using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skill_Exchange.Application.Validator
{
    /*public class AppUserAddValidator : IValidator<AppUser>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppUserAddValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsValidAsync(CreateDTO DTO, CancellationToken cancellationToken)
        {
            // Example: email must be unique
            var exists = await _unitOfWork.Users.GetByEmailAsync(DTO.Email);
            return exists == null;
        }
    }*/

}
