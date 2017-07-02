using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class RefferalMap : IEntityTypeConfiguration<Referral>
    {
        public void Map(EntityTypeBuilder<Referral> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasAlternateKey(r => r.Email);
            builder.HasAlternateKey(r => r.Code);

            builder.Property(r => r.Code).IsRequired();
            builder.Property(r => r.ReferrerUserId).IsRequired();
            builder.Property(r => r.ExpiryDate).IsRequired(false);

            builder.HasOne(r => r.Referrer).WithMany().HasForeignKey(r => r.ReferrerUserId);
        }
    }
}
