
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
        // One-to-Many relationship between AppUser and RatingAndFeedback (rating and feedback are not removed if user is removed)
        builder.HasMany(user => user.RatingsGiven)
       .WithOne(rate => rate.FromUser)
       .HasForeignKey(rate => rate.FromUserId)
       .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(user => user.RatingsReceived)
       .WithOne(rate => rate.ToUser)
       .HasForeignKey(rate => rate.ToUserId)
       .OnDelete(DeleteBehavior.NoAction);


        // One-to-Many relationship between AppUser and Request (request is removed if user is removed)
        builder.HasMany(u => u.RequestsSent)
       .WithOne(r => r.Sender)
       .HasForeignKey(r => r.SenderId)
       .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.RequestsReceived)
       .WithOne(r => r.Reciever)
       .HasForeignKey(r => r.RecieverId)
       .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many relationship between AppUser and Conversation (conversation is not removed if user is removed ,but conversation is relevant to no user)
        builder.HasMany(u => u.ConversationsAsA)
       .WithOne(c => c.ParticipantA)
       .HasForeignKey(c => c.ParticipantAId)
       .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.ConversationsAsB)
       .WithOne(c => c.ParticipantB)
       .HasForeignKey(c => c.ParticipantBId)
       .OnDelete(DeleteBehavior.NoAction);

        // Many-to-many between AppUser and Skill through UserSkill (if we removed user , so all UserSkills should be removed)
        builder.HasMany(u => u.Skills)
            .WithMany(s => s.Users)
            .UsingEntity<UserSkills>(
                j => j
                    .HasOne(us => us.Skill)
                    .WithMany()
                    .HasForeignKey(us => us.SkillId)
                    .OnDelete(DeleteBehavior.Cascade),   // Delete relation if Skill is deleted
                j => j
                    .HasOne(us => us.User)
                    .WithMany()
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade),   // Delete relation if User is deleted
                j =>
                {
                    j.ToTable("UserSkills");
                });

        // Many-to-many between AppUser and Notification (if we removed user , so all UserNotifications should be removed)
        builder.HasMany(u => u.Notifications)
            .WithMany(n => n.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserNotifications",
                j => j
                    .HasOne<Notification>()
                    .WithMany()
                    .HasForeignKey("NotificationId")
                    .OnDelete(DeleteBehavior.Cascade),   // If Notification is deleted
                j => j
                    .HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade));  // If User is deleted

        // Many-to-many self-relationship (AppUser ↔ AppUser) through UserFriends (if we removed user , so all UsersFriends should be removed)
        builder.HasMany(u => u.Friends)
            .WithMany(u => u.FriendOf)
            .UsingEntity<Dictionary<string, object>>(
                "UserFriends",
                j => j
                    .HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey("FriendId")
                    .OnDelete(DeleteBehavior.NoAction),   // If Friend is deleted
                j => j
                    .HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade));  // If User is deleted


        // Indexing
        builder.HasIndex(u => u.Email).IsUnique(); // for login / identity
        builder.HasIndex(u => u.PhoneNumber).IsUnique(); // prevent duplicate phone numbers
        builder.HasIndex(u => u.LastActiveAt); // for sorting/filtering by recent activity
        builder.HasIndex(u => new { u.FirstName, u.LastName }); // optimize full name search


    }
}
