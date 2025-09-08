
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class UserNotificationsConfig : IEntityTypeConfiguration<UserNotifications>
{
    public void Configure(EntityTypeBuilder<UserNotifications> builder)
    {
        throw new NotImplementedException();
    }
}
