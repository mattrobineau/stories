using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stories.Data.Entities.Mappings
{
    public interface IEntityTypeConfiguration<T> where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}
