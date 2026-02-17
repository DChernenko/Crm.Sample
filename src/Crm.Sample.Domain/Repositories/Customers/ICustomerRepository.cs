using Crm.Sample.Domain.Entities.Customers;
using Crm.Sample.Domain.Repositories.Base;

namespace Crm.Sample.Domain.Repositories.Customers
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<int> CountNewCustomersAsync(DateTime fromDateUtc);
    }
}
