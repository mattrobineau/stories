using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class StoryMap : IEntityTypeConfiguration<Story>
    {
        public void Map(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Url);
            builder.Property(s => s.Title).IsRequired();
            builder.Property(s => s.UserId).IsRequired();
            
            builder.HasOne(s => s.User).WithMany().HasForeignKey(e => e.UserId);
            builder.HasOne(s => s.Score).WithOne(sc => sc.Story).HasForeignKey<StoryScore>(s => s.StoryId);
            builder.HasMany(s => s.Comments).WithOne(c => c.Story).HasForeignKey(c => c.StoryId);
            builder.HasMany(s => s.Votes).WithOne(v => v.Story).HasForeignKey(v => v.StoryId);
        }
    }
}
