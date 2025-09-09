
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skill_Exchange.Domain.Entities;
namespace Skill_Exchange.Infrastructure.Configurations;

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).ValueGeneratedOnAdd();

        builder.Property(user => user.FirstName)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(user => user.LastName)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(user => user.PhoneNumber)
        .IsRequired()
        .HasMaxLength(32);

        builder.Property(user => user.DateOfBirth)
        .HasConversion(
        v => v.ToDateTime(TimeOnly.MinValue),
        v => DateOnly.FromDateTime(v))
        .IsRequired();

        builder.Property(user => user.Bio)
        .HasMaxLength(1000);

        builder.Property(user => user.LastActiveAt)
        .IsRequired();

        builder.Property(user => user.ProfileImageUrl)
        .HasMaxLength(512);

        //relationships
        // One-to-Many relationship between AppUser and RatingAndFeedback
        builder.HasMany(user => user.RatingsGiven)
       .WithOne(rate => rate.FromUser)
       .HasForeignKey(rate => rate.FromUserId);

        builder.HasMany(user => user.RatingsReceived)
       .WithOne(rate => rate.ToUser)
       .HasForeignKey(rate => rate.ToUserId);

        // One-to-Many relationship between AppUser and Request
        builder.HasMany(u => u.RequestsSent)
       .WithOne(r => r.Sender)
       .HasForeignKey(r => r.SenderId);

        builder.HasMany(u => u.RequestsReceived)
       .WithOne(r => r.Reciever)
       .HasForeignKey(r => r.RecieverId);

        // Many-to-Many relationship between AppUser and Conversation
        builder.HasMany(u => u.ConversationsAsA)
       .WithOne(c => c.ParticipantA)
       .HasForeignKey(c => c.ParticipantAId);

        builder.HasMany(u => u.ConversationsAsB)
       .WithOne(c => c.ParticipantB)
       .HasForeignKey(c => c.ParticipantBId);

        //many-to-many relationship between AppUser and Skill through UserSkill
        builder.HasMany(u => u.Skills)
       .WithMany(s => s.Users)
       .UsingEntity<UserSkills>();

        // Many-to-Many relationship between AppUser and Notification
        builder.HasMany(u => u.Notifications)
       .WithMany(n => n.Users)
       .UsingEntity(join => join.ToTable("UserNotifications"));

        // Many-to-Many relationship between AppUser and AppUser through UserFriend
        builder.HasMany(u => u.Friends)
         .WithMany(u => u.FriendOf)
         .UsingEntity(join => join.ToTable("UserFriends"));
    }
}
