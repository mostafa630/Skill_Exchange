
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


        
    }
}
