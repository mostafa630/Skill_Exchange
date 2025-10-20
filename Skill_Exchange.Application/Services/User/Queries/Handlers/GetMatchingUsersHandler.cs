using MediatR;
using Skill_Exchange.Application.DTOs.User;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using DomainSkill = Skill_Exchange.Domain.Entities.Skill; // alias to avoid naming conflict

namespace Skill_Exchange.Application.Services.Users.Queries
{
    public class GetMatchingUsersHandler : IRequestHandler<GetMatchingUsers, List<UserMatchDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMatchingUsersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserMatchDTO>> Handle(GetMatchingUsers request, CancellationToken cancellationToken)
        {
            var usersRepo = _unitOfWork.Users;
            var skillsRepo = _unitOfWork.GetRepository<DomainSkill>();

            var targetUser = await usersRepo.GetByIdAsync(request.UserId);
            if (targetUser == null)
                return new List<UserMatchDTO>();

            var skillsToLearn = await GetSkillsToLearnAsync(request, targetUser, skillsRepo);
            var allUsers = await usersRepo.GetAllAsync();

            var matches = new List<UserMatchDTO>();

            foreach (var user in allUsers.Where(u => u.Id != request.UserId))
            {
                double matchScore = CalculateMatchScore(targetUser, user, skillsToLearn);
                if (matchScore > 0)
                {
                    matches.Add(new UserMatchDTO
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        MatchScore = matchScore,
                        Skills = user.Skills.Select(s => s.Name).ToList(),
                        LastActiveAt = user.LastActiveAt
                    });
                }
            }

            return matches
                .OrderByDescending(m => m.MatchScore)
                .Take(request.Top)
                .ToList();
        }

        // ------------------------- Private Helpers ---------------------------- //

        private async Task<List<string>> GetSkillsToLearnAsync(GetMatchingUsers request, AppUser user, IGenericRepository<DomainSkill> skillsRepo)
        {
            if (request.SkillsToLearn != null && request.SkillsToLearn.Any())
                return request.SkillsToLearn.Select(s => s.ToLower()).ToList();

            var userSkills = user.Skills.Select(s => s.Name.ToLower()).ToList();
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
