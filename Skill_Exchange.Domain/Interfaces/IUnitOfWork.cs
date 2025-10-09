using Skill_Exchange.Domain.Entities;

namespace Skill_Exchange.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGenericRepository<Skill> Skills { get; }
        IGenericRepository<SkillCategory> SkillCategories { get; }
        IUserSkillRepository UserSkills { get; }
        IGenericRepository<RatingAndFeedback> RatingsAndFeedbacks { get; }
        IRequestRepository Requests { get; }
        IGenericRepository<Conversation> Conversations { get; }
        IGenericRepository<Notification> Notifications { get; }
        IPendingVerificationRepository PendingVerifications { get; }
        Task<int> CompleteAsync();
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
