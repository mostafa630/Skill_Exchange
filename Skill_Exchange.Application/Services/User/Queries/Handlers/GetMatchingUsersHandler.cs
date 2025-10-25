using MediatR;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Enums;
using Skill_Exchange.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using DomainSkill = Skill_Exchange.Domain.Entities.Skill;

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
                // 1) my skills to learn
                var query = _unitOfWork.UserSkills
                    .AsQueryable()
                    .Where(us => us.UserId == request.UserId && us.Purpose == ExchangePurpose.Learning);

                // If specific skill IDs are provided, filter by them
                if (request.SkillsToLearn is not null && request.SkillsToLearn.Any())
                {
                    query = query.Where(us => request.SkillsToLearn.Contains(us.SkillId));
                }

                var myLearnSkills = await query
                    .Select(us => new { us.SkillId, us.YearsOfExperience })
                    .ToListAsync(cancellationToken);
                // 2) my skills to teach
                var myTeachSkills = await _unitOfWork.UserSkills
                    .AsQueryable()
                    .Where(us => us.UserId == request.UserId && us.Purpose == ExchangePurpose.Teaching)
                    .Select(us => new { us.SkillId, us.YearsOfExperience })
                    .ToListAsync(cancellationToken);

                // 3) Other users' skills to learn
                var otherUsersSkills = await _unitOfWork.UserSkills
                     .AsQueryable()
                     .Include(us => us.User) 
                     .Where(us => us.UserId != request.UserId) 
                     .GroupBy(us => new
                     {
                         us.UserId,
                         us.User.FirstName,
                         us.User.LastName,
                         us.User.ProfileImageUrl
                     })
                     .Select(g => new
                     {
                         UserId = g.Key.UserId,
                         FullName = g.Key.FirstName + " " + g.Key.LastName,
                         ProfileImageUrl = g.Key.ProfileImageUrl,
                         SkillsToLearn = g
                             .Where(us => us.Purpose == ExchangePurpose.Learning)
                             .Select(us => new { us.SkillId, us.YearsOfExperience })
                             .ToList(),
                         SkillsToTeach = g
                             .Where(us => us.Purpose == ExchangePurpose.Teaching)
                             .Select(us => new { us.SkillId, us.YearsOfExperience })
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
                    if (totalMatch > 0)
                    {
                        userMatches.Add(new UserMatchDTO
                        {
                            UserId = otherUser.UserId,
                            MatchScore = totalMatch,
                            ImageUrl = otherUser.ProfileImageUrl,
                            FullName = otherUser.FullName
                        });
                    }
                }
                userMatches = userMatches
                    .OrderByDescending(um => um.MatchScore)
                    .Take(request.Top)
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
        /*private async Task<List<string>> GetSkillsToLearnAsync(GetMatchingUsers request, AppUser user, IGenericRepository<DomainSkill> skillsRepo)
        {
            if (request.SkillsToLearn != null && request.SkillsToLearn.Any())
                return request.SkillsToLearn.Select(s => s.ToLower()).ToList();

            var userSkills = user.Skills.Where(s => s.Name.ToLower == request.sk);
            var allSkills = await skillsRepo.GetAllAsync();

            return allSkills
                .Select(s => s.Name.ToLower())
                .Except(userSkills)
                .Take(5)
                .ToList();
        }

        private double CalculateMatchScore(AppUser targetUser, AppUser otherUser, List<string> skillsToLearn)
        {
            double teachMatch = CalculateTeachMatch(otherUser, skillsToLearn);
            double activityScore = CalculateActivityScore(otherUser.LastActiveAt);
            double friendshipPenalty = targetUser.Friends.Any(f => f.Id == otherUser.Id) ? -0.2 : 0;

            double totalScore = (teachMatch * 0.7) + (activityScore * 0.3) + friendshipPenalty;
            return Math.Max(0, Math.Min(totalScore, 1));
        }

        private double CalculateTeachMatch(AppUser otherUser, List<string> skillsToLearn)
        {
            if (!otherUser.Skills.Any() || !skillsToLearn.Any())
                return 0;

            var otherSkills = otherUser.Skills.Select(s => s.Name.ToLower()).ToHashSet();
            var intersection = otherSkills.Intersect(skillsToLearn).Count();
            var union = otherSkills.Union(skillsToLearn).Count();

            return union == 0 ? 0 : (double)intersection / union;
        }

        private double CalculateActivityScore(DateTime lastActive)
        {
            var days = (DateTime.UtcNow - lastActive).TotalDays;
            return days switch
            {
                <= 1 => 1.0,
                <= 7 => 0.8,
                <= 30 => 0.5,
                <= 90 => 0.3,
                _ => 0.1
            };
        }*/
    }
}
