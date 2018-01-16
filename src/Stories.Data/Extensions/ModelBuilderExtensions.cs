using Microsoft.EntityFrameworkCore;
using System;

namespace Stories.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder RegisterEntityMapping<TEntity, TMapping>(this ModelBuilder builder)
        where TMapping : Entities.Mappings.IEntityTypeConfiguration<TEntity>
        where TEntity : class
        {
            var mapper = (Entities.Mappings.IEntityTypeConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            mapper.Map(builder.Entity<TEntity>());
            return builder;
        }
    }
}
