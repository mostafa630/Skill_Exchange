
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class ConversationConfig : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.IsActive).IsRequired();

        // Ignore Messages (because they're in MongoDB)
        builder.Ignore(c => c.Messages);


        // Indexing

        // Prevents duplicate conversations between the same two users
        builder.HasIndex(c => new { c.ParticipantAId, c.ParticipantBId }).IsUnique();
        // Faster lookups for conversations where the user is ParticipantA
        builder.HasIndex(c => c.ParticipantAId);
        // Faster lookups for conversations where the user is ParticipantB
        builder.HasIndex(c => c.ParticipantBId);



    }
}
