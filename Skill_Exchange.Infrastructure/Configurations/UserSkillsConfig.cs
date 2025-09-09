
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

        // Indexing
        builder.HasIndex(us => new { us.UserId, us.SkillId }).IsUnique(); // useful for searching all users of a specific skill
        builder.HasIndex(us => us.SkillId); // fast lookup by skill
        builder.HasIndex(us => us.UserId); // fast lookup by user


    }
}
