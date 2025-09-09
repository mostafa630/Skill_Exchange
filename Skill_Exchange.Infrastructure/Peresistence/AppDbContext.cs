using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Skill_Exchange.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill_Exchange.Infrastructure.Peresistence
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RatingAndFeedback> RatingsAndFeedbacks { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillCategory> SkillCategory { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserSkills> UserSkills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // --- Seeding users + roles ---
            var hasher = new PasswordHasher<AppUser>();

            var user1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var user2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            // Users
            var user1 = new AppUser
            {
                Id = user1Id,
                FirstName = "Farag",
                LastName = "Elyan",
                UserName = "farag.elyan",
                NormalizedUserName = "FARAG.ELYAN",
                Email = "faragelyan722@gmail.com",
                NormalizedEmail = "FARAGELYAN722@GMAIL.COM",
                PhoneNumber = "0100000001",
                DateOfBirth = new DateOnly(2000, 1, 1),
                Bio = "Seeded Admin user",
                LastActiveAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user1.PasswordHash = hasher.HashPassword(user1, "Password123!");

            var user2 = new AppUser
            {
                Id = user2Id,
                FirstName = "Test",
                LastName = "User",
                UserName = "test.user",
                NormalizedUserName = "TEST.USER",
                Email = "faragelyan76@gmail.com",
                NormalizedEmail = "FARAGELYAN76@GMAIL.COM",
                PhoneNumber = "0100000002",
                DateOfBirth = new DateOnly(2001, 2, 2),
                Bio = "Seeded normal user",
                LastActiveAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user2.PasswordHash = hasher.HashPassword(user2, "Password123!");

            builder.Entity<AppUser>().HasData(user1, user2);

            // Roles
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // User-Role assignments
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = user1Id, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = user2Id, RoleId = userRoleId }
            );
        }

    }
}