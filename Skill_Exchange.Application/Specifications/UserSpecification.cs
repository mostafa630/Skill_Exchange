using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Application.Specifications
{
    public class UserSpecification : BaseSpecification<AppUser>
    {
        private UserSpecification()
        {

        }
        public static UserSpecification Build(UserFilterDTO filter, UserIncludesDTO includes, PaginationDto pagination)
        {
            var userSpec = new UserSpecification();
            userSpec = filter_process(userSpec, filter);
            userSpec = include_process(userSpec, includes);
            userSpec = applyPagination(userSpec, pagination);
            return userSpec;
        }

        private static UserSpecification filter_process(UserSpecification userSpec, UserFilterDTO filter)
        {
            userSpec.And(u => u.Id != filter.UserId); // exclude my self

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

            // Filter using Firend relationships
            if (filter.GetFriendsIadded == true && filter.GetFriendsAddedMe == true)
            {
                // Get users I added OR users who added me
                userSpec.And(u =>
                    u.Friends.Any(f => f.Id == filter.UserId) ||
                    u.FriendOf.Any(f => f.Id == filter.UserId));
            }
            else if (filter.GetFriendsIadded == true)
            {
                // I added them → they have me in FriendOf
                userSpec.And(u => u.FriendOf.Any(f => f.Id == filter.UserId));
            }
            else if (filter.GetFriendsAddedMe == true)
            {
                // They added me → they have me in Friends
                userSpec.And(u => u.Friends.Any(f => f.Id == filter.UserId));
            }
            else if (filter.GetFriendsIadded == false && filter.GetFriendsAddedMe == false)
            {
                // Neither added nor added me — users I have no friend relationship with
                userSpec.And(u =>
                    !u.Friends.Any(f => f.Id == filter.UserId) &&
                    !u.FriendOf.Any(f => f.Id == filter.UserId));
            }
            return userSpec;
        }

        private static UserSpecification include_process(UserSpecification userSpec, UserIncludesDTO includes)
        {
            if (includes.RatingsGiven)
                userSpec.AddInclude(u => u.RatingsGiven);

            if (includes.RatingsReceived)
                userSpec.AddInclude(u => u.RatingsReceived);

            if (includes.RequestsSent)
                userSpec.AddInclude(u => u.RequestsSent);

            if (includes.RequestsReceived)
                userSpec.AddInclude(u => u.RequestsReceived);

            if (includes.ConversationsAsA)
                userSpec.AddInclude(u => u.ConversationsAsA);

            if (includes.ConversationsAsB)
                userSpec.AddInclude(u => u.ConversationsAsB);

            if (includes.Skills)
                userSpec.AddInclude(u => u.Skills);

            if (includes.Notifications)
                userSpec.AddInclude(u => u.Notifications);

            if (includes.FriendOf)
                userSpec.AddInclude(u => u.FriendOf);

            if (includes.Friends)
                userSpec.AddInclude(u => u.Friends);

            return userSpec;
        }
        private static UserSpecification applyPagination(UserSpecification userSpec, PaginationDto pagination)
        {
            if (pagination.ApplyPagination)
            {
                userSpec.ApplyPaging(pagination.Skip, pagination.Take);
                return userSpec;
            }
            return userSpec;
        }
    }
}