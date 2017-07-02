using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class CommentMap : IEntityTypeConfiguration<Comment>
    {
        public void Map(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.StoryId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.ParentCommentId).IsRequired(false);
            
            builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies).HasForeignKey(c => c.ParentCommentId);
            builder.HasOne(c => c.Score).WithOne(cs => cs.Comment).HasForeignKey<CommentScore>(s => s.CommentId);
            builder.HasOne(c => c.Story).WithMany().HasForeignKey(c => c.StoryId);
            // Cyclical reference with Comment->User->Comments->User etc
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.HasMany(c => c.Votes).WithOne(c => c.Comment).HasForeignKey(v => v.CommentId);            
        }        
    }
}
