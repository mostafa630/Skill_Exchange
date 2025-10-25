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
                    .Where(us => us.UserId != request.UserId) // exclude the current user
                    .GroupBy(us => us.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
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
            }
            catch
            {

            }


        }

        // ------------------------- Private Helpers ---------------------------- //

        private async Task<List<string>> GetSkillsToLearnAsync(GetMatchingUsers request, AppUser user, IGenericRepository<DomainSkill> skillsRepo)
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
        }
    }
}
