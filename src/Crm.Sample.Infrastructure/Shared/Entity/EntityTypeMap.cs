using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Crm.Sample.Infrastructure.Shared.Entity
{
    public class EntityTypeMap<TEntity>(ModelBuilder builder) where TEntity : class
    {
        public EntityTypeBuilder<TEntity> EntityTypeBuilder { get; } = builder.Entity<TEntity>();
        public EntityTypeBuilder<TEntity> ToTable(string name, string schema) => EntityTypeBuilder.ToTable(name, schema);
        public PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) => EntityTypeBuilder.Property(propertyExpression);
        public KeyBuilder HasKey(Expression<Func<TEntity, object>> keyExpression) => EntityTypeBuilder.HasKey(keyExpression);        
    }
}
