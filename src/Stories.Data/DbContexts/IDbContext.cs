using Microsoft.EntityFrameworkCore;
using Stories.Data.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stories.Data.DbContexts
{
    public interface IDbContext : IDisposable
    {        
        DbSet<Comment> Comments { get; set; }
        DbSet<CommentScore> CommentScores { get; set; }
        DbSet<StoryScore> StoryScores { get; set; }
        DbSet<Story> Stories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserSettings> UserSettings { get; set; }
        DbSet<Vote> Votes { get; set; }
        DbSet<Referral> Referrals { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserBan> UserBans { get; set; }
        DbSet<UserRole> UserRoles { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
