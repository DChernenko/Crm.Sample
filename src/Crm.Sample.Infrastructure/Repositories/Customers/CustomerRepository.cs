using Crm.Sample.Domain.Entities.Customers;
using Crm.Sample.Domain.Repositories.Customers;
using Crm.Sample.Infrastructure.Persistence;
using Crm.Sample.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Crm.Sample.Infrastructure.Repositories.Customers
{
    public class CustomerRepository(AppDbContext context)
        : BaseRepository<Customer>(context), ICustomerRepository
    {
        public Task<int> CountNewCustomersAsync(DateTime fromDateUtc)
            => _context.Customers
                .Where(c => c.CreateDate >= fromDateUtc)
                .CountAsync();
    }
}
