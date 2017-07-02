using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class CommentScoreMap : IEntityTypeConfiguration<CommentScore>
    {
        public void Map(EntityTypeBuilder<CommentScore> builder)
        {
            builder.HasKey(cs => cs.Id);
            builder.Property(cs => cs.CommentId).IsRequired();
            builder.Property(cs => cs.Value).IsRequired();

        }
    }
}
