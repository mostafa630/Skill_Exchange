using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Specifications
{
    public class UserSpecification : BaseSpecification<AppUser>
    {
        private UserSpecification()
        {

        }
        public static UserSpecification Build(UserFilterDTO filter)
        {
            var userSpec = new UserSpecification();

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                userSpec.And(u => u.FirstName.Contains(filter.FirstName));

            if (!string.IsNullOrWhiteSpace(filter.LastName))
                userSpec.And(u => u.LastName.Contains(filter.LastName));

            if (!string.IsNullOrWhiteSpace(filter.Email))
                userSpec.And(u => u.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                userSpec.And(u => u.PhoneNumber.Contains(filter.PhoneNumber));

            if (!string.IsNullOrWhiteSpace(filter.BioContains))
                userSpec.And(u => u.Bio.Contains(filter.BioContains));

            if (filter.ActiveAfter.HasValue)
                userSpec.And(u => u.LastActiveAt >= filter.ActiveAfter.Value);

            if (filter.ActiveBefore.HasValue)
                userSpec.And(u => u.LastActiveAt <= filter.ActiveBefore.Value);

            if (filter.HasProfileImage.HasValue)
            {
                if (filter.HasProfileImage.Value)
                    userSpec.And(u => u.ProfileImageUrl != null && u.ProfileImageUrl != "");
                else
                    userSpec.And(u => u.ProfileImageUrl == null || u.ProfileImageUrl == "");
            }
            return userSpec;
        }

    }
}