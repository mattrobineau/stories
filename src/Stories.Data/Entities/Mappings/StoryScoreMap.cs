using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class StoryScoreMap : IEntityTypeConfiguration<StoryScore>
    {
        public void Map(EntityTypeBuilder<StoryScore> builder)
        {
            builder.HasKey(sc => sc.Id);
            builder.Property(sc => sc.StoryId).IsRequired();
            builder.Property(sc => sc.Value).IsRequired();


        }
    }
}
