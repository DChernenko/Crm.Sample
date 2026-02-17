using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Crm.Sample.Infrastructure.Shared.Entity
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<TProperty> HasColumnTypeDate<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
            => propertyBuilder.HasColumnType("date");

        public static PropertyBuilder<TProperty> HasColumnTypeDateTime<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
            => propertyBuilder.HasColumnType("datetime2(0)");
    }
}
