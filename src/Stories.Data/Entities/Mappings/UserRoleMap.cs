using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Map(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.Property(ur => ur.RoleId).IsRequired();
            builder.Property(ur => ur.UserId).IsRequired();

            builder.HasOne(ur => ur.User).WithMany(ur => ur.Roles).HasForeignKey(ur => ur.UserId);
            builder.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId);
        }
    }
}
