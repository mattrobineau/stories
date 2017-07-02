using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class UserBanMap : IEntityTypeConfiguration<UserBan>
    {
        public void Map(EntityTypeBuilder<UserBan> builder)
        {
            builder.HasKey(ub => ub.Id);
            builder.Property(ub => ub.UserId).IsRequired();
            builder.Property(ub => ub.Reason).IsRequired();
            builder.Property(ub => ub.Notes).IsRequired(false);
            builder.Property(ub => ub.ExpiryDate).IsRequired(false);

            builder.HasOne(ub => ub.User).WithMany().HasForeignKey(ub => ub.UserId);
            builder.HasOne(ub => ub.BannedByUser).WithMany().HasForeignKey(ub => ub.BannedByUserId);
        }
    }
}
