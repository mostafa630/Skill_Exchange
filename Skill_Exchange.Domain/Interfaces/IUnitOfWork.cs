using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGenericRepository<Skill> Skills { get; }
        IGenericRepository<SkillCategory> SkillCategories { get; }
        IGenericRepository<UserSkills> UserSkills { get; }
        IGenericRepository<RatingAndFeedback> RatingsAndFeedbacks { get; }
        IGenericRepository<Request> Requests { get; }
        IGenericRepository<Conversation> Conversations { get; }
        IGenericRepository<Notification> Notifications { get; }
        IPendingVerificationRepository PendingVerifications { get; }
        Task<int> CompleteAsync();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
