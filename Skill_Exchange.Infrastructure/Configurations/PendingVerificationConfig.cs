using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class PendingVerificationConfig : IEntityTypeConfiguration<PendingVerification>
{
    public void Configure(EntityTypeBuilder<PendingVerification> builder)
    {
        builder.ToTable("PendingVerifications");
        builder.HasKey(pv => pv.Id);
        builder.Property(pv => pv.Id).ValueGeneratedOnAdd();
        builder.Property(pv => pv.Id).IsRequired();

        builder.Property(pv => pv.Email).IsRequired();

        builder.Property(pv => pv.VerificationCode).IsRequired();

        builder.Property(pv => pv.Expiry).IsRequired();

        builder.Property(pv => pv.IsConfirmed).IsRequired().HasDefaultValue(false);

    }
}