using Crm.Sample.Domain.Entities.Customers;
using Crm.Sample.Infrastructure.Configuration.Base;
using Microsoft.EntityFrameworkCore;

namespace Crm.Sample.Infrastructure.Configuration
{
    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public CustomerConfiguration(ModelBuilder builder) : base(builder)
        {
            ToTable("Customers", "Core");

            Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            Property(c => c.LastName).IsRequired().HasMaxLength(100);
            Property(c => c.Phone).IsRequired().HasMaxLength(20);
            Property(c => c.Email).IsRequired().HasMaxLength(256);
        }
    }
}
