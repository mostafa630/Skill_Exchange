using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<AppUser> Users { get; }
        IGenericRepository<Skill> Skills { get; }
        IGenericRepository<SkillCategory> SkillCategories { get; }
        IGenericRepository<UserSkills> UserSkills { get; }
        IGenericRepository<RatingAndFeedback> RatingsAndFeedbacks { get; }
        IGenericRepository<Request> Requests { get; }
        IGenericRepository<Conversation> Conversations { get; }
        IGenericRepository<Notification> Notifications { get; }
        Task<int> CompleteAsync();



    }
}
