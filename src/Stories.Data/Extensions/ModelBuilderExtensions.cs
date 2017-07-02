using Microsoft.EntityFrameworkCore;
using Stories.Data.Entities.Mappings;
using System;

namespace Stories.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder RegisterEntityMapping<TEntity, TMapping>(this ModelBuilder builder)
        where TMapping : IEntityTypeConfiguration<TEntity>
        where TEntity : class
        {
            var mapper = (IEntityTypeConfiguration<TEntity>)Activator.CreateInstance(typeof(TMapping));
            mapper.Map(builder.Entity<TEntity>());
            return builder;
        }
    }
}
