using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure.Peresistence;

namespace Skill_Exchange.Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IGenericRepository<AppUser> Users { get; private set; }
        public IGenericRepository<Skill> Skills { get; private set; }
        public IGenericRepository<SkillCategory> SkillCategories { get; private set; }
        public IGenericRepository<UserSkills> UserSkills { get; private set; }
        public IGenericRepository<RatingAndFeedback> RatingsAndFeedbacks { get; private set; }
        public IGenericRepository<Request> Requests { get; private set; }
        public IGenericRepository<Conversation> Conversations { get; private set; }
        public IGenericRepository<Notification> Notifications { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new GenericRepository<AppUser>(context);
            Skills = new GenericRepository<Skill>(context);
            SkillCategories = new GenericRepository<SkillCategory>(context);
            UserSkills = new GenericRepository<UserSkills>(context);
            RatingsAndFeedbacks = new GenericRepository<RatingAndFeedback>(context);
            Requests = new GenericRepository<Request>(context);
            Conversations = new GenericRepository<Conversation>(context);
            Notifications = new GenericRepository<Notification>(context);
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
