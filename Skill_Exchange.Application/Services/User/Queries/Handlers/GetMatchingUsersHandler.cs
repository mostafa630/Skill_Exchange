using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Application.DTOs.Skill;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Enums;
using Skill_Exchange.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Skill_Exchange.Application.Services.Users.Queries
{
    public class GetMatchingUsersHandler : IRequestHandler<GetMatchingUsers, Result<List<UserMatchDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMatchingUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<UserMatchDTO>>> Handle(GetMatchingUsers request, CancellationToken cancellationToken)
        {
            try
            {
                // 1) My skills to learn
                var query = _unitOfWork.UserSkills
                    .AsQueryable()
                    .Include(us => us.Skill)
                    .Where(us => us.UserId == request.UserId && us.Purpose == ExchangePurpose.Learning);

                if (request.SkillsToLearn is not null && request.SkillsToLearn.Any())
                    query = query.Where(us => request.SkillsToLearn.Contains(us.SkillId));

                var myLearnSkills = await query
                    .Select(us => new SkillDTO
                    {
                        SkillId = us.SkillId,
                        SkillName = us.Skill.Name,
                        YearsOfExperience = us.YearsOfExperience
                    })
                    .ToListAsync(cancellationToken);

                // 2) My skills to teach
                var myTeachSkills = await _unitOfWork.UserSkills
                    .AsQueryable()
                    .Include(us => us.Skill)
                    .Where(us => us.UserId == request.UserId && us.Purpose == ExchangePurpose.Teaching)
                    .Select(us => new SkillDTO
                    {
                        SkillId = us.SkillId,
                        SkillName = us.Skill.Name,
                        YearsOfExperience = us.YearsOfExperience
                    })
                    .ToListAsync(cancellationToken);

                // 3) Other users' skills
                var otherUsersSkills = await _unitOfWork.UserSkills
                     .AsQueryable()
                     .Include(us => us.User)
                     .Include(us => us.Skill)
                     .Where(us => us.UserId != request.UserId)
                     .GroupBy(us => new
                     {
                         us.UserId,
                         us.User.FirstName,
                         us.User.LastName,
                         us.User.ProfileImageUrl,
                         us.User.Bio
                     })
                     .Select(g => new
                     {
                         UserId = g.Key.UserId,
                         FullName = g.Key.FirstName + " " + g.Key.LastName,
                         ProfileImageUrl = g.Key.ProfileImageUrl,
                         Bio = g.Key.Bio,
                         SkillsToLearn = g
                             .Where(us => us.Purpose == ExchangePurpose.Learning)
                             .Select(us => new SkillDTO
                             {
                                 SkillId = us.SkillId,
                                 SkillName = us.Skill.Name,
                                 YearsOfExperience = us.YearsOfExperience
                             })
                             .ToList(),
                         SkillsToTeach = g
                             .Where(us => us.Purpose == ExchangePurpose.Teaching)
                             .Select(us => new SkillDTO
                             {
                                 SkillId = us.SkillId,
                                 SkillName = us.Skill.Name,
                                 YearsOfExperience = us.YearsOfExperience
                             })
                             .ToList()
                     })
                     .ToListAsync(cancellationToken);

                // 4) Calculate match scores
                var userMatches = new List<UserMatchDTO>();
                foreach (var otherUser in otherUsersSkills)
                {
                    double learnMatch = CalculateSkillMatch(
                        myLearnSkills.Select(s => (s.SkillId, s.YearsOfExperience)).ToList(),
                        otherUser.SkillsToTeach.Select(s => (s.SkillId, s.YearsOfExperience)).ToList());

                    double teachMatch = CalculateSkillMatch(
                        myTeachSkills.Select(s => (s.SkillId, s.YearsOfExperience)).ToList(),
                        otherUser.SkillsToLearn.Select(s => (s.SkillId, s.YearsOfExperience)).ToList());

                    double totalMatch = (learnMatch + teachMatch) / 2.0;

                    if (totalMatch >= 0)
                    {
                        userMatches.Add(new UserMatchDTO
                        {
                            UserId = otherUser.UserId,
                            FullName = otherUser.FullName,
                            ImageUrl = otherUser.ProfileImageUrl,
                            Bio = otherUser.Bio,
                            MatchScore = totalMatch,
                            SkillsToTeach = otherUser.SkillsToTeach,
                            SkillsToLearn = otherUser.SkillsToLearn
                        });
                    }
                }

                if (request.Pagination is not null && request.Pagination.ApplyPagination == true)
                {
                    userMatches = userMatches.Skip(request.Pagination.Skip).Take(request.Pagination.Take).ToList();
                }
                // 5) Return top matches
                userMatches = userMatches
                    .OrderByDescending(um => um.MatchScore)
                    .ToList();

                return Result<List<UserMatchDTO>>.Ok(userMatches);
            }
            catch
            {
                return Result<List<UserMatchDTO>>.Fail("Failed to get matching users.");
            }
        }

        // ------------------------- Private Helpers ---------------------------- //
        private double CalculateSkillMatch(
            List<(Guid SkillId, int YearsOfExperience)> mySkills,
            List<(Guid SkillId, int YearsOfExperience)> otherSkills)
        {
            if (!mySkills.Any() || !otherSkills.Any())
                return 0;

            var mySkillSet = mySkills.ToDictionary(s => s.SkillId, s => s.YearsOfExperience);
            var otherSkillSet = otherSkills.ToDictionary(s => s.SkillId, s => s.YearsOfExperience);
            var intersection = mySkillSet.Keys.Intersect(otherSkillSet.Keys).ToList();

            if (!intersection.Any())
                return 0;

            double score = 0;
            foreach (var skillId in intersection)
            {
                var myExp = mySkillSet[skillId];
                var otherExp = otherSkillSet[skillId];
                score += 1.0 / (1 + Math.Abs(myExp - otherExp));
            }

            return score / intersection.Count;
        }
    }
}
