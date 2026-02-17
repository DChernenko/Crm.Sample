using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace Crm.Sample.Infrastructure.Shared.Entity
{
    public class EntityTypeMap<TEntity>(ModelBuilder builder) where TEntity : class
    {
        public const string ExcludeFromCodeCoverageJustification = "Entity mapping configuration is EF specific and does not require unit testing";
        public EntityTypeBuilder<TEntity> EntityTypeBuilder { get; } = builder.Entity<TEntity>();
        public EntityTypeBuilder<TEntity> ToTable(string name, string schema) => EntityTypeBuilder.ToTable(name, schema);
        public PropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression) => EntityTypeBuilder.Property(propertyExpression);
        public KeyBuilder HasKey(Expression<Func<TEntity, object>> keyExpression) => EntityTypeBuilder.HasKey(keyExpression);
        public IndexBuilder<TEntity> HasIndex(Expression<Func<TEntity, object>> indexExpression) => EntityTypeBuilder.HasIndex(indexExpression);
        public EntityTypeBuilder<TEntity> Ignore(Expression<Func<TEntity, object>> propertyExpression) => EntityTypeBuilder.Ignore(propertyExpression);
        public CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> collection = null) where TRelatedEntity : class => EntityTypeBuilder.HasMany(collection);
        public ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> reference = null) where TRelatedEntity : class => EntityTypeBuilder.HasOne(reference);
    }
}
