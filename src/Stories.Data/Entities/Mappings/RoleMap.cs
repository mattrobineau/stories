using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Map(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);            
        }
    }
}
