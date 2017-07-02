using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Stories.Data.Entities;
using Stories.Data.Entities.Mappings;
using Stories.Data.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stories.Data.DbContexts
{
    public class StoriesDbContext : DbContext, IDbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<StoryScore> StoryScores { get; set; }
        public DbSet<CommentScore> CommentScores { get; set; }
        public DbSet<Story> Stories { get; set; }    
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Referral> Referrals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserBan> UserBans { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public StoriesDbContext(DbContextOptions<StoriesDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Remove Cascade Delete on all entities
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.RegisterEntityMapping<Comment, CommentMap>();
            builder.RegisterEntityMapping<Story, StoryMap>();
            builder.RegisterEntityMapping<User, UserMap>();
            builder.RegisterEntityMapping<Role, RoleMap>();
            builder.RegisterEntityMapping<UserRole, UserRoleMap>();
            builder.RegisterEntityMapping<Vote, VoteMap>();

            //builder.Entity<User>(b => b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()"));
            //builder.Entity<Role>(b => b.Property(r => r.Id).HasDefaultValueSql("newsequentialid()"));
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var time = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is ITimestamp && (e.State == EntityState.Added || e.State == EntityState.Modified)))
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property("CreatedDate").CurrentValue = time;
                }

                entry.Property("ModifiedDate").CurrentValue = time;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
