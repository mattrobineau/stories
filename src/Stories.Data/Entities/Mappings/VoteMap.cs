using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class VoteMap : IEntityTypeConfiguration<Vote>
    {
        public void Map(EntityTypeBuilder<Vote> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(v => v.UserId).IsRequired();
            builder.Property(v => v.StoryId).IsRequired(false);
            builder.Property(v => v.CommentId).IsRequired(false);

            builder.HasOne(e => e.Story).WithMany(s => s.Votes).HasForeignKey(v => v.StoryId).IsRequired(false);
            builder.HasOne(e => e.Comment).WithMany(c => c.Votes).HasForeignKey(v => v.CommentId).IsRequired(false);
            builder.HasOne(v => v.User).WithMany(u => u.Votes).HasForeignKey(v => v.UserId);
        }
    }
}
