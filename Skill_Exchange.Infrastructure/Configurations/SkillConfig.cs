
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class SkillConfig : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(s => s.IsPredefined)
        .IsRequired().HasDefaultValue(0);

        builder.Property(s => s.CreatedBy)
        .HasMaxLength(100);

    }
}
