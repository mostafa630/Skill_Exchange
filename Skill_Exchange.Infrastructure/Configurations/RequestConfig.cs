
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class RequestConfig : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.RespondedAt).IsRequired(false);

        // Indexing
        builder.HasIndex(r => r.SenderId);     // who sent the request
        builder.HasIndex(r => r.RecieverId);   // who received the request
        builder.HasIndex(r => r.Status);       // useful for filtering (Pending / Accepted / Rejected)
        builder.HasIndex(r => new { r.SenderId, r.RecieverId })
            .IsUnique(false); // allow multiple requests but makes lookups faster
    }
}

