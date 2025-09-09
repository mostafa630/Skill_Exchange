
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class SkillCategoryConfig : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> builder)
    {
        builder.HasKey(sc => sc.Id);
        builder.Property(sc => sc.Id).ValueGeneratedOnAdd();

        builder.Property(sc => sc.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(sc => sc.Description)
               .HasMaxLength(1000);

        //relationship
        builder.HasMany(sc => sc.Skills)
               .WithOne(s => s.SkillCategory)
               .HasForeignKey(s => s.SkillCategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        // Indexing
        builder.HasIndex(sc => sc.Name).IsUnique(); // useful for searching category by name

    }
}
