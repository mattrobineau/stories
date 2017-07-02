using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Map(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Email);

            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordSalt).IsRequired();
            builder.Property(u => u.Username).IsRequired();
            builder.Property(u => u.Email).IsRequired();

            // Navigation
            builder.HasMany(u => u.Roles).WithOne(u => u.User).HasForeignKey(r => r.UserId);
            builder.HasOne(u => u.Settings).WithMany().HasForeignKey(u => u.SettingsId).IsRequired(false);
            builder.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            builder.HasMany(u => u.Stories).WithOne(c => c.User).HasForeignKey(s => s.UserId);
            builder.HasMany(u => u.Referrals).WithOne(r => r.Referrer).HasForeignKey(r => r.ReferrerUserId);
        }
    }
}
