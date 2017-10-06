using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class FlagMap
    {
        public void Map(EntityTypeBuilder<Flag> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(v => v.UserId).IsRequired();
            builder.Property(v => v.StoryId).IsRequired(false);
            builder.Property(v => v.CommentId).IsRequired(false);

            builder.HasOne(e => e.Story).WithMany(s => s.Flags).HasForeignKey(v => v.StoryId).IsRequired(false);
            builder.HasOne(e => e.Comment).WithMany(c => c.Flags).HasForeignKey(v => v.CommentId).IsRequired(false);
            builder.HasOne(v => v.User).WithMany(u => u.Flags).HasForeignKey(v => v.UserId);
        }
    }
}
