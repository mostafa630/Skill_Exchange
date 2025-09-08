
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class UserSkillsConfig : IEntityTypeConfiguration<UserSkills>
{
    public void Configure(EntityTypeBuilder<UserSkills> builder)
    {
        //builder.HasKey(us => new { us.UserId, us.SkillId });

        builder.Property(us => us.YearsOfExperience)
               .IsRequired();

        builder.Property(us => us.Description)
               .HasMaxLength(1000);

        builder.Property(us => us.Purpose)
               .IsRequired();
    }
}
