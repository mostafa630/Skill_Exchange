
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Title).IsRequired().HasMaxLength(100);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(500);
        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.RefrenceId).IsRequired();

        // Indexing
        builder.HasIndex(n => n.CreatedAt); // for sorting notifications by time
        builder.HasIndex(n => n.Title); // useful if filtering by type(info, alert, etc.)

    }
}
