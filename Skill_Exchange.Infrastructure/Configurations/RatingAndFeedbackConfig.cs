
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class RatingAndFeedbackConfig : IEntityTypeConfiguration<RatingAndFeedback>
{
    public void Configure(EntityTypeBuilder<RatingAndFeedback> builder)
    {
        builder.HasKey(rf => rf.Id);
        builder.Property(rf => rf.Id).ValueGeneratedOnAdd();

        builder.Property(rf => rf.CreatedAt)
        .IsRequired();

        builder.Property(rf => rf.Score);

        builder.Property(rf => rf.Feedback)
        .HasMaxLength(2000);

        // Indexing
        builder.HasIndex(n => n.CreatedAt); // for sorting notifications by time

    }
}