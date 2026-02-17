using Crm.Sample.Domain.Entities.Base;
using Crm.Sample.Infrastructure.Shared.Entity;
using Microsoft.EntityFrameworkCore;

namespace Crm.Sample.Infrastructure.Configuration.Base
{
    public abstract class BaseEntityConfiguration<TEntity> : EntityTypeMap<TEntity>
        where TEntity : BaseEntity
    {
        protected BaseEntityConfiguration(ModelBuilder builder) : base(builder)
        {
            HasKey(t => t.Id);

            Property(t => t.Id).ValueGeneratedOnAdd();

            Property(t => t.ModifiedDate).HasColumnTypeDateTime();                        
        }
    }
}
